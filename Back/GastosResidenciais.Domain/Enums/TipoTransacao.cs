namespace GastosResidenciais.Domain.Enums;

/// <summary>
/// Define o tipo de uma transação financeira.
/// Utilizado para classificar se o lançamento representa
/// uma entrada (Receita) ou uma saída (Despesa) de recursos.
/// </summary>
public enum TipoTransacao
{
    /// <summary>Saída de dinheiro — ex.: conta de luz, aluguel.</summary>
    Despesa = 0,

    /// <summary>Entrada de dinheiro — ex.: salário, rendimento.</summary>
    Receita = 1
}
