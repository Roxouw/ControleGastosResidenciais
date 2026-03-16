using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Repositories;

/// <summary>
/// Implementação genérica do repositório.
/// Centraliza as operações CRUD básicas, evitando duplicação de código
/// nos repositórios específicos (Pessoa, Categoria, Transacao).
///
/// Todas as operações de escrita chamam SaveChangesAsync() para persistir
/// as alterações imediatamente — padrão Unit of Work simples.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet   = context.Set<T>();
    }

    /// <summary>Retorna todos os registros da tabela correspondente a T.</summary>
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    /// <summary>Busca um registro pelo Id (GUID). Retorna null se não encontrado.</summary>
    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    /// <summary>Adiciona uma nova entidade e persiste no banco.</summary>
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>Marca a entidade como modificada e persiste as alterações.</summary>
    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>Remove a entidade do contexto e persiste a deleção.</summary>
    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
