namespace GastosResidenciais.Domain.Enums;

/// <summary>
/// Define para qual finalidade uma categoria pode ser utilizada.
/// Controla quais categorias aparecem disponíveis ao criar uma transação,
/// impedindo associações incoerentes (ex.: categoria "Salário" em uma Despesa).
/// </summary>
public enum FinalidadeCategoria
{
    /// <summary>Categoria exclusiva para transações do tipo Despesa.</summary>
    Despesa = 0,

    /// <summary>Categoria exclusiva para transações do tipo Receita.</summary>
    Receita = 1,

    /// <summary>Categoria aceita tanto em Despesas quanto em Receitas.</summary>
    Ambas = 2
}
