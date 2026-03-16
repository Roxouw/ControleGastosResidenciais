using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Services;

/// <summary>
/// Implementação do serviço de relatórios.
/// Agrega as transações de todas as pessoas e categorias para gerar
/// os totais de receitas, despesas e saldo (receita - despesa).
/// </summary>
public class RelatorioService : IRelatorioService
{
    private readonly IPessoaRepository    _pessoaRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public RelatorioService(
        IPessoaRepository pessoaRepository,
        ICategoriaRepository categoriaRepository,
        ITransacaoRepository transacaoRepository)
    {
        _pessoaRepository    = pessoaRepository;
        _categoriaRepository = categoriaRepository;
        _transacaoRepository = transacaoRepository;
    }

    /// <summary>
    /// Gera o relatório de totais por pessoa.
    ///
    /// Algoritmo:
    /// 1. Busca todas as pessoas cadastradas.
    /// 2. Busca todas as transações com detalhes (eager load).
    /// 3. Para cada pessoa, filtra suas transações e calcula receitas e despesas.
    /// 4. Calcula o saldo líquido (receitas - despesas) de cada pessoa.
    /// 5. Soma todos os valores para o total geral consolidado.
    /// </summary>
    public async Task<RelatorioResumoPessoasDto> GetTotaisPorPessoaAsync()
    {
        var pessoas     = (await _pessoaRepository.GetAllAsync()).ToList();
        var transacoes  = (await _transacaoRepository.GetAllWithDetailsAsync()).ToList();

        var linhas = pessoas.Select(pessoa =>
        {
            // Filtra as transações desta pessoa específica
            var txPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id).ToList();

            var receitas = txPessoa
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var despesas = txPessoa
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            return new RelatorioPessoaDto(
                pessoa.Id,
                pessoa.Nome,
                pessoa.Idade,
                receitas,
                despesas,
                receitas - despesas   // Saldo = Receitas - Despesas
            );
        }).ToList();

        // Totais gerais: consolida os valores de todas as pessoas
        var totaisGerais = new TotaisDto(
            TotalReceitas: linhas.Sum(l => l.TotalReceitas),
            TotalDespesas: linhas.Sum(l => l.TotalDespesas),
            Saldo:         linhas.Sum(l => l.Saldo)
        );

        return new RelatorioResumoPessoasDto(linhas, totaisGerais);
    }

    /// <summary>
    /// Gera o relatório de totais por categoria.
    ///
    /// Algoritmo análogo ao de pessoas, mas agrupando por categoria.
    /// Inclui categorias sem transações com valores zerados,
    /// garantindo que todas as categorias apareçam no relatório.
    /// </summary>
    public async Task<RelatorioResumoCategoriaDto> GetTotaisPorCategoriaAsync()
    {
        var categorias  = (await _categoriaRepository.GetAllAsync()).ToList();
        var transacoes  = (await _transacaoRepository.GetAllWithDetailsAsync()).ToList();

        var linhas = categorias.Select(categoria =>
        {
            var txCategoria = transacoes.Where(t => t.CategoriaId == categoria.Id).ToList();

            var receitas = txCategoria
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var despesas = txCategoria
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            return new RelatorioCategoriaDto(
                categoria.Id,
                categoria.Descricao,
                receitas,
                despesas,
                receitas - despesas
            );
        }).ToList();

        var totaisGerais = new TotaisDto(
            TotalReceitas: linhas.Sum(l => l.TotalReceitas),
            TotalDespesas: linhas.Sum(l => l.TotalDespesas),
            Saldo:         linhas.Sum(l => l.Saldo)
        );

        return new RelatorioResumoCategoriaDto(linhas, totaisGerais);
    }
}
