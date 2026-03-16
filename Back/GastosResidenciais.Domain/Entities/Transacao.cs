using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Domain.Entities;

/// <summary>
/// Representa um lançamento financeiro (receita ou despesa) de uma pessoa.
/// É o núcleo do sistema: vincula uma pessoa a um valor monetário
/// classificado por tipo e categoria.
/// </summary>
public class Transacao
{
    /// <summary>Identificador único gerado automaticamente (GUID).</summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição do lançamento financeiro.
    /// Limitada a 400 caracteres conforme especificação.
    /// Exemplos: "Conta de água — janeiro", "Salário fevereiro".
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor monetário da transação. Deve ser um número positivo (> 0).
    /// Validado na camada de Application antes de persistir.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Tipo da transação: Despesa (saída) ou Receita (entrada).
    /// Restringe as categorias disponíveis e, para menores de idade,
    /// somente Despesa é permitida.
    /// </summary>
    public TipoTransacao Tipo { get; set; }

    // ── Chave estrangeira e navegação: Categoria ──────────────────────────

    /// <summary>FK para a categoria desta transação.</summary>
    public Guid CategoriaId { get; set; }

    /// <summary>
    /// Categoria associada. Deve ser compatível com o Tipo da transação
    /// (validado na camada de Application).
    /// </summary>
    public Categoria Categoria { get; set; } = null!;

    // ── Chave estrangeira e navegação: Pessoa ─────────────────────────────

    /// <summary>FK para a pessoa dona desta transação.</summary>
    public Guid PessoaId { get; set; }

    /// <summary>
    /// Pessoa proprietária do lançamento.
    /// Configurada com exclusão em cascata no banco de dados:
    /// ao deletar a Pessoa, esta transação também é removida.
    /// </summary>
    public Pessoa Pessoa { get; set; } = null!;
}
