using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Services;

/// <summary>
/// Implementação do serviço de transações.
/// É aqui que se concentram as principais regras de negócio do sistema:
///   1. Menores de 18 anos só podem ter transações do tipo Despesa.
///   2. A categoria escolhida deve ser compatível com o tipo da transação.
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository    _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public TransacaoService(
        ITransacaoRepository transacaoRepository,
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository    = pessoaRepository;
        _categoriaRepository = categoriaRepository;
    }

    /// <summary>
    /// Retorna todas as transações com os dados de pessoa e categoria incluídos.
    /// O eager loading é feito no repositório com Include() do EF Core.
    /// </summary>
    public async Task<IEnumerable<TransacaoResponseDto>> GetAllAsync()
    {
        var transacoes = await _transacaoRepository.GetAllWithDetailsAsync();
        return transacoes.Select(MapToDto);
    }

    /// <summary>
    /// Cria uma transação aplicando as seguintes validações de negócio:
    ///
    /// [1] Existência da Pessoa — HTTP 404 se não encontrada.
    /// [2] Existência da Categoria — HTTP 404 se não encontrada.
    /// [3] Restrição de menor de idade — HTTP 422 se a pessoa tem menos de 18
    ///     e a transação é do tipo Receita.
    /// [4] Compatibilidade Categoria × Tipo — HTTP 422 se a categoria não
    ///     aceita o tipo de transação informado.
    /// </summary>
    public async Task<TransacaoResponseDto> CreateAsync(CreateTransacaoDto dto)
    {
        // [1] Verifica se a pessoa existe
        var pessoa = await _pessoaRepository.GetByIdAsync(dto.PessoaId)
            ?? throw new KeyNotFoundException($"Pessoa com Id '{dto.PessoaId}' não encontrada.");

        // [2] Verifica se a categoria existe
        var categoria = await _categoriaRepository.GetByIdAsync(dto.CategoriaId)
            ?? throw new KeyNotFoundException($"Categoria com Id '{dto.CategoriaId}' não encontrada.");

        // [3] Regra: menor de 18 anos só pode registrar Despesas
        if (pessoa.EhMenorDeIdade && dto.Tipo == TipoTransacao.Receita)
        {
            throw new InvalidOperationException(
                $"A pessoa '{pessoa.Nome}' tem {pessoa.Idade} anos. " +
                "Menores de 18 anos só podem registrar transações do tipo Despesa.");
        }

        // [4] Regra: categoria deve ser compatível com o tipo da transação
        if (!categoria.CompatívelCom(dto.Tipo))
        {
            throw new InvalidOperationException(
                $"A categoria '{categoria.Descricao}' tem finalidade '{categoria.Finalidade}' " +
                $"e não pode ser usada em uma transação do tipo '{dto.Tipo}'.");
        }

        // Todas as validações passaram — cria a entidade
        var transacao = new Transacao
        {
            Id          = Guid.NewGuid(),
            Descricao   = dto.Descricao.Trim(),
            Valor       = dto.Valor,
            Tipo        = dto.Tipo,
            CategoriaId = dto.CategoriaId,
            PessoaId    = dto.PessoaId
        };

        var criada = await _transacaoRepository.AddAsync(transacao);

        // Popula as propriedades de navegação para o mapeamento do DTO
        criada.Pessoa    = pessoa;
        criada.Categoria = categoria;

        return MapToDto(criada);
    }

    // ── Mapeamento privado ────────────────────────────────────────────────

    /// <summary>
    /// Converte a entidade de transação para DTO, incluindo nomes
    /// desnormalizados de pessoa e categoria para facilitar o front-end.
    /// </summary>
    private static TransacaoResponseDto MapToDto(Transacao t) =>
        new(t.Id, t.Descricao, t.Valor, t.Tipo,
            t.CategoriaId, t.Categoria.Descricao,
            t.PessoaId,    t.Pessoa.Nome);
}
