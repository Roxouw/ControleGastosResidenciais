using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.Application.Interfaces;

/// <summary>
/// Contrato de serviço para operações de gerenciamento de transações.
/// </summary>
public interface ITransacaoService
{
    /// <summary>Retorna todas as transações cadastradas, incluindo dados de pessoa e categoria.</summary>
    Task<IEnumerable<TransacaoResponseDto>> GetAllAsync();

    /// <summary>
    /// Cria uma nova transação aplicando as regras de negócio:
    /// 1. Valida se a pessoa existe.
    /// 2. Valida se a categoria existe.
    /// 3. Se a pessoa for menor de 18 anos, rejeita transações do tipo Receita.
    /// 4. Valida se a categoria é compatível com o tipo de transação.
    /// 
    /// Lança <see cref="InvalidOperationException"/> em caso de violação de regra.
    /// Lança <see cref="KeyNotFoundException"/> se pessoa ou categoria não existirem.
    /// </summary>
    Task<TransacaoResponseDto> CreateAsync(CreateTransacaoDto dto);
}
