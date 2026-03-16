using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Interfaces;

// ── Repositório Genérico ──────────────────────────────────────────────────────

/// <summary>
/// Contrato genérico de repositório.
/// Define as operações básicas de persistência que qualquer repositório deve implementar.
/// O uso de interfaces aqui permite trocar o banco de dados sem alterar a lógica de negócio.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

// ── Repositórios Específicos ──────────────────────────────────────────────────

/// <summary>
/// Repositório de pessoas — estende as operações genéricas sem adições por ora.
/// Pronto para receber queries específicas no futuro (ex.: busca por nome).
/// </summary>
public interface IPessoaRepository : IRepository<Pessoa> { }

/// <summary>
/// Repositório de categorias com operação especializada de filtragem por compatibilidade.
/// </summary>
public interface ICategoriaRepository : IRepository<Categoria>
{
    /// <summary>
    /// Retorna apenas as categorias compatíveis com o tipo de transação informado.
    /// </summary>
    Task<IEnumerable<Categoria>> GetCompatibleAsync(TipoTransacao tipo);
}

/// <summary>
/// Repositório de transações com carregamento eager de entidades relacionadas.
/// </summary>
public interface ITransacaoRepository : IRepository<Transacao>
{
    /// <summary>
    /// Retorna todas as transações incluindo os dados de Pessoa e Categoria
    /// via eager loading (Include no EF Core), evitando o problema N+1.
    /// </summary>
    Task<IEnumerable<Transacao>> GetAllWithDetailsAsync();
}
