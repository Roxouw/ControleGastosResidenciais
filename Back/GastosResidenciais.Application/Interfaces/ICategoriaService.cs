using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Interfaces;

/// <summary>
/// Contrato de serviço para operações de gerenciamento de categorias.
/// </summary>
public interface ICategoriaService
{
    /// <summary>Retorna todas as categorias cadastradas.</summary>
    Task<IEnumerable<CategoriaResponseDto>> GetAllAsync();

    /// <summary>
    /// Retorna apenas as categorias compatíveis com o tipo de transação informado.
    /// Regra: Despesa → finalidades Despesa e Ambas.
    ///        Receita → finalidades Receita e Ambas.
    /// Utilizado pelo front-end para filtrar o select de categorias
    /// conforme o tipo de transação selecionado pelo usuário.
    /// </summary>
    Task<IEnumerable<CategoriaResponseDto>> GetCompatibleAsync(TipoTransacao tipo);

    /// <summary>
    /// Cria uma nova categoria com os dados fornecidos.
    /// Retorna o DTO da categoria criada com o Id gerado.
    /// </summary>
    Task<CategoriaResponseDto> CreateAsync(CreateCategoriaDto dto);
}
