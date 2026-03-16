using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Data;

/// <summary>
/// Contexto principal do Entity Framework Core.
/// Representa a sessão com o banco de dados SQLite e
/// registra todas as entidades do domínio como DbSets.
///
/// As configurações de mapeamento (chaves, relacionamentos, constraints)
/// são definidas em classes separadas (EntityTypeConfiguration),
/// mantendo o contexto limpo e as configurações coesas.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>Tabela de pessoas cadastradas.</summary>
    public DbSet<Pessoa> Pessoas { get; set; }

    /// <summary>Tabela de categorias financeiras.</summary>
    public DbSet<Categoria> Categorias { get; set; }

    /// <summary>Tabela de transações financeiras.</summary>
    public DbSet<Transacao> Transacoes { get; set; }

    /// <summary>
    /// Aplica as configurações de cada entidade ao modelo do EF Core.
    /// Cada entidade tem sua própria classe de configuração seguindo
    /// o padrão IEntityTypeConfiguration<T>.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PessoaConfiguration());
        modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        modelBuilder.ApplyConfiguration(new TransacaoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
