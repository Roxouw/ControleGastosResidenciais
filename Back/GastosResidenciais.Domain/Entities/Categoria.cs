using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Domain.Entities;

/// <summary>
/// Representa uma categoria de classificação para transações financeiras.
/// A finalidade define em quais tipos de transação essa categoria pode ser usada,
/// evitando inconsistências semânticas no lançamento dos dados.
/// </summary>
public class Categoria
{
    /// <summary>Identificador único gerado automaticamente (GUID).</summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da categoria.
    /// Limitada a 400 caracteres conforme especificação.
    /// Exemplos: "Alimentação", "Salário", "Transporte".
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Define para qual finalidade esta categoria pode ser utilizada:
    /// Despesa, Receita ou Ambas.
    /// </summary>
    public FinalidadeCategoria Finalidade { get; set; }

    /// <summary>
    /// Coleção de transações que usam esta categoria.
    /// Propriedade de navegação do Entity Framework Core.
    /// </summary>
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

    /// <summary>
    /// Verifica se esta categoria é compatível com o tipo de transação informado.
    /// Regra: Despesa aceita Finalidade=Despesa ou Finalidade=Ambas.
    ///        Receita aceita Finalidade=Receita ou Finalidade=Ambas.
    /// </summary>
    public bool CompatívelCom(TipoTransacao tipo) =>
        Finalidade == FinalidadeCategoria.Ambas ||
        (tipo == TipoTransacao.Despesa && Finalidade == FinalidadeCategoria.Despesa) ||
        (tipo == TipoTransacao.Receita && Finalidade == FinalidadeCategoria.Receita);
}
