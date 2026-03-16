namespace GastosResidenciais.Application.DTOs;

// ── REQUEST DTOs ──────────────────────────────────────────────────────────────

/// <summary>
/// Dados necessários para criar uma nova pessoa.
/// Utilizado como body no POST /api/pessoas.
/// </summary>
public record CreatePessoaDto(
    string Nome,
    int Idade
);

/// <summary>
/// Dados utilizados para atualizar uma pessoa existente.
/// Utilizado como body no PUT /api/pessoas/{id}.
/// </summary>
public record UpdatePessoaDto(
    string Nome,
    int Idade
);

// ── RESPONSE DTOs ─────────────────────────────────────────────────────────────

/// <summary>
/// Representação de uma pessoa retornada pela API.
/// Expõe todos os campos públicos sem expor a entidade de domínio diretamente.
/// </summary>
public record PessoaResponseDto(
    Guid Id,
    string Nome,
    int Idade
);
