using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.Application.Interfaces;

/// <summary>
/// Contrato de serviço para operações de gerenciamento de pessoas.
/// A interface desacopla a lógica de negócio da implementação concreta,
/// permitindo substituição e teste unitário sem dependência de infraestrutura.
/// </summary>
public interface IPessoaService
{
    /// <summary>Retorna todas as pessoas cadastradas.</summary>
    Task<IEnumerable<PessoaResponseDto>> GetAllAsync();

    /// <summary>
    /// Retorna uma pessoa pelo seu identificador.
    /// Lança <see cref="KeyNotFoundException"/> se não encontrada.
    /// </summary>
    Task<PessoaResponseDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Cria uma nova pessoa com os dados fornecidos.
    /// Retorna o DTO da pessoa criada com o Id gerado.
    /// </summary>
    Task<PessoaResponseDto> CreateAsync(CreatePessoaDto dto);

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// Lança <see cref="KeyNotFoundException"/> se não encontrada.
    /// </summary>
    Task<PessoaResponseDto> UpdateAsync(Guid id, UpdatePessoaDto dto);

    /// <summary>
    /// Remove uma pessoa e todas as suas transações (cascade).
    /// Lança <see cref="KeyNotFoundException"/> se não encontrada.
    /// </summary>
    Task DeleteAsync(Guid id);
}
