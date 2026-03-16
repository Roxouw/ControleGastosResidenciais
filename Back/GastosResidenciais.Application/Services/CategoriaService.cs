using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.Services;

/// <summary>
/// Implementação do serviço de categorias.
/// Fornece listagem completa e filtrada de categorias,
/// além da criação de novas entradas.
/// </summary>
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Retorna todas as categorias mapeadas para DTO.</summary>
    public async Task<IEnumerable<CategoriaResponseDto>> GetAllAsync()
    {
        var categorias = await _repository.GetAllAsync();
        return categorias.Select(MapToDto);
    }

    /// <summary>
    /// Retorna somente as categorias compatíveis com o tipo de transação.
    /// Regra de compatibilidade:
    ///   - Tipo Despesa → Finalidade Despesa ou Ambas
    ///   - Tipo Receita → Finalidade Receita ou Ambas
    /// Essa filtragem é usada pelo front-end no select de categorias
    /// para guiar o usuário e evitar seleções inválidas.
    /// </summary>
    public async Task<IEnumerable<CategoriaResponseDto>> GetCompatibleAsync(TipoTransacao tipo)
    {
        var categorias = await _repository.GetCompatibleAsync(tipo);
        return categorias.Select(MapToDto);
    }

    /// <summary>
    /// Cria uma nova categoria gerando automaticamente o Id (GUID).
    /// </summary>
    public async Task<CategoriaResponseDto> CreateAsync(CreateCategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Id         = Guid.NewGuid(),
            Descricao  = dto.Descricao.Trim(),
            Finalidade = dto.Finalidade
        };

        var criada = await _repository.AddAsync(categoria);
        return MapToDto(criada);
    }

    // ── Mapeamento privado ────────────────────────────────────────────────

    private static CategoriaResponseDto MapToDto(Categoria c) =>
        new(c.Id, c.Descricao, c.Finalidade);
}
