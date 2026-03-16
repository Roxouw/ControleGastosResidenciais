using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;
using GastosResidenciais.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Repositories;

// ── PessoaRepository ──────────────────────────────────────────────────────────

/// <summary>
/// Repositório de pessoas.
/// Herda todas as operações genéricas de Repository&lt;Pessoa&gt;.
/// </summary>
public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
{
    public PessoaRepository(AppDbContext context) : base(context) { }
}

// ── CategoriaRepository ───────────────────────────────────────────────────────

/// <summary>
/// Repositório de categorias.
/// Adiciona a query especializada de filtragem por compatibilidade de tipo.
/// </summary>
public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Retorna categorias compatíveis com o tipo de transação informado.
    /// O filtro é executado no banco de dados via SQL gerado pelo EF Core.
    /// </summary>
    public async Task<IEnumerable<Categoria>> GetCompatibleAsync(TipoTransacao tipo)
    {
        return await _dbSet
            .Where(c =>
                c.Finalidade == FinalidadeCategoria.Ambas ||
                (tipo == TipoTransacao.Despesa && c.Finalidade == FinalidadeCategoria.Despesa) ||
                (tipo == TipoTransacao.Receita && c.Finalidade == FinalidadeCategoria.Receita))
            .OrderBy(c => c.Descricao)
            .ToListAsync();
    }
}

// ── TransacaoRepository ───────────────────────────────────────────────────────

/// <summary>
/// Repositório de transações.
/// Nota: O SQLite não suporta ORDER BY em colunas do tipo decimal diretamente
/// via EF Core. Por isso, o ordenamento é feito em memória após o ToListAsync().
/// </summary>
public class TransacaoRepository : Repository<Transacao>, ITransacaoRepository
{
    public TransacaoRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Retorna todas as transações com Include de Pessoa e Categoria.
    /// O ordenamento por Valor é aplicado em memória (LINQ to Objects)
    /// para contornar a limitação do SQLite com decimal em ORDER BY.
    /// </summary>
    public async Task<IEnumerable<Transacao>> GetAllWithDetailsAsync()
    {
        var transacoes = await _dbSet
            .Include(t => t.Pessoa)
            .Include(t => t.Categoria)
            .ToListAsync();

        // Ordenamento em memória — evita NotSupportedException do SQLite com decimal
        return transacoes.OrderByDescending(t => t.Valor);
    }
}