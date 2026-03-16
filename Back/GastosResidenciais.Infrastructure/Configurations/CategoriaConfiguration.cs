using GastosResidenciais.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastosResidenciais.Infrastructure.Configurations;

/// <summary>
/// Configuração do mapeamento EF Core para a entidade Categoria.
/// Define a tabela, chave primária e constraints de coluna.
/// </summary>
public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        // Chave primária — GUID gerado pela aplicação
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        // Descrição: obrigatória, máx. 400 chars conforme especificação
        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(400);

        // Finalidade: armazenada como inteiro no SQLite (0=Despesa, 1=Receita, 2=Ambas)
        builder.Property(c => c.Finalidade)
            .IsRequired()
            .HasConversion<int>();

        // Método de instância não deve ser mapeado para coluna
        builder.Ignore("CompatívelCom");
    }
}
