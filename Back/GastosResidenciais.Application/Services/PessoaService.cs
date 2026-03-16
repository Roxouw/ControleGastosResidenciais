using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;

namespace GastosResidenciais.Application.Services;

/// <summary>
/// Implementação do serviço de pessoas.
/// Contém a lógica de negócio para CRUD de pessoas,
/// delegando a persistência ao repositório injetado.
/// </summary>
public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _repository;

    public PessoaService(IPessoaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retorna todas as pessoas mapeadas para DTO de resposta.
    /// O mapeamento manual evita dependência de bibliotecas externas (AutoMapper)
    /// e deixa explícito o que é exposto pela API.
    /// </summary>
    public async Task<IEnumerable<PessoaResponseDto>> GetAllAsync()
    {
        var pessoas = await _repository.GetAllAsync();
        return pessoas.Select(MapToDto);
    }

    /// <summary>
    /// Busca uma pessoa pelo Id. Lança exceção se não encontrar,
    /// o que é capturado pelo middleware global e retorna HTTP 404.
    /// </summary>
    public async Task<PessoaResponseDto> GetByIdAsync(Guid id)
    {
        var pessoa = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Pessoa com Id '{id}' não encontrada.");

        return MapToDto(pessoa);
    }

    /// <summary>
    /// Cria uma nova pessoa gerando automaticamente o Id (GUID).
    /// O Id é responsabilidade da camada de serviço/domínio, não do cliente.
    /// </summary>
    public async Task<PessoaResponseDto> CreateAsync(CreatePessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Id   = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade
        };

        var criada = await _repository.AddAsync(pessoa);
        return MapToDto(criada);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// Busca a entidade, aplica as alterações e persiste.
    /// </summary>
    public async Task<PessoaResponseDto> UpdateAsync(Guid id, UpdatePessoaDto dto)
    {
        var pessoa = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Pessoa com Id '{id}' não encontrada.");

        pessoa.Nome  = dto.Nome.Trim();
        pessoa.Idade = dto.Idade;

        var atualizada = await _repository.UpdateAsync(pessoa);
        return MapToDto(atualizada);
    }

    /// <summary>
    /// Remove a pessoa do banco. As transações associadas são deletadas
    /// automaticamente pelo banco de dados via FOREIGN KEY CASCADE,
    /// configurado no EntityTypeConfiguration da Transacao.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var pessoa = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Pessoa com Id '{id}' não encontrada.");

        await _repository.DeleteAsync(pessoa);
    }

    // ── Mapeamento privado ────────────────────────────────────────────────

    /// <summary>
    /// Converte a entidade de domínio para o DTO de resposta.
    /// Centralizado aqui para manter consistência entre todos os métodos.
    /// </summary>
    private static PessoaResponseDto MapToDto(Pessoa p) =>
        new(p.Id, p.Nome, p.Idade);
}
