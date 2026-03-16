using GastosResidenciais.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastosResidenciais.Infrastructure.Configurations;

/// <summary>
/// Configuração do mapeamento EF Core para a entidade Pessoa.
/// Define a tabela, chave primária e constraints de coluna.
/// Separar a configuração da entidade mantém o DbContext limpo
/// e agrupa as responsabilidades de mapeamento por entidade.
/// </summary>
public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("Pessoas");

        // Chave primária — GUID gerado pela aplicação (não pelo banco)
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        // Nome: obrigatório, máx. 200 chars conforme especificação
        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        // Idade: obrigatória
        builder.Property(p => p.Idade)
            .IsRequired();

        // Propriedade calculada — não é persistida no banco
        builder.Ignore(p => p.EhMenorDeIdade);
    }
}
