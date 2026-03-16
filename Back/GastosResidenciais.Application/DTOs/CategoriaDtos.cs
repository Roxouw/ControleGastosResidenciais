using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.DTOs;

// ── REQUEST DTOs ──────────────────────────────────────────────────────────────

/// <summary>
/// Dados necessários para criar uma nova categoria.
/// Utilizado como body no POST /api/categorias.
/// </summary>
public record CreateCategoriaDto(
    string Descricao,
    FinalidadeCategoria Finalidade
);

// ── RESPONSE DTOs ─────────────────────────────────────────────────────────────

/// <summary>
/// Representação de uma categoria retornada pela API.
/// </summary>
public record CategoriaResponseDto(
    Guid Id,
    string Descricao,
    FinalidadeCategoria Finalidade
);
