using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Domain.Entities;

/// <summary>
/// Representa uma pessoa cadastrada no sistema.
/// Cada pessoa pode ter múltiplas transações financeiras associadas.
/// Ao ser deletada, todas as suas transações são removidas em cascata.
/// </summary>
public class Pessoa
{
    /// <summary>Identificador único gerado automaticamente (GUID).</summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome completo da pessoa.
    /// Limitado a 200 caracteres conforme especificação.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa em anos.
    /// Pessoas menores de 18 anos só podem registrar despesas.
    /// </summary>
    public int Idade { get; set; }

    /// <summary>
    /// Coleção de transações financeiras vinculadas a esta pessoa.
    /// Propriedade de navegação do Entity Framework Core.
    /// </summary>
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

    /// <summary>
    /// Indica se a pessoa é menor de idade (menos de 18 anos).
    /// Utilizado pela camada de serviço para validar o tipo de transação permitida.
    /// </summary>
    public bool EhMenorDeIdade => Idade < 18;
}
