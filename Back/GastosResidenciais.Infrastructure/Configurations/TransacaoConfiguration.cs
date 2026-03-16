using GastosResidenciais.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastosResidenciais.Infrastructure.Configurations;

/// <summary>
/// Configuração do mapeamento EF Core para a entidade Transacao.
/// Esta é a configuração mais importante do sistema pois define:
/// - Os dois relacionamentos com FK (Pessoa e Categoria)
/// - O comportamento de DELETE CASCADE para Pessoa
/// </summary>
public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacoes");

        // Chave primária — GUID gerado pela aplicação
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        // Descrição: obrigatória, máx. 400 chars conforme especificação
        builder.Property(t => t.Descricao)
            .IsRequired()
            .HasMaxLength(400);

        // Valor: obrigatório, precisão decimal adequada para valores monetários
        builder.Property(t => t.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Tipo: armazenado como inteiro (0=Despesa, 1=Receita)
        builder.Property(t => t.Tipo)
            .IsRequired()
            .HasConversion<int>();

        // ── Relacionamento: Transacao → Pessoa (N:1) ──────────────────────
        // DELETE CASCADE: ao deletar uma Pessoa, todas suas Transações são
        // automaticamente removidas pelo banco, atendendo à especificação.
        builder.HasOne(t => t.Pessoa)
            .WithMany(p => p.Transacoes)
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Relacionamento: Transacao → Categoria (N:1) ───────────────────
        // Restrict: não permite deletar uma Categoria que tenha transações.
        // (Categorias não têm operação de delete na especificação,
        //  mas protege a integridade referencial.)
        builder.HasOne(t => t.Categoria)
            .WithMany(c => c.Transacoes)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
