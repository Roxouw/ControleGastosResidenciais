using GastosResidenciais.Domain.Enums;

namespace GastosResidenciais.Application.DTOs;

// ── REQUEST DTOs ──────────────────────────────────────────────────────────────

/// <summary>
/// Dados necessários para criar uma nova transação.
/// Utilizado como body no POST /api/transacoes.
/// 
/// Regras de negócio validadas na camada de Application:
/// - Se a pessoa for menor de 18 anos, apenas Tipo=Despesa é aceito.
/// - A Categoria escolhida deve ser compatível com o Tipo da transação.
/// - Valor deve ser positivo (> 0).
/// </summary>
public record CreateTransacaoDto(
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    Guid CategoriaId,
    Guid PessoaId
);

// ── RESPONSE DTOs ─────────────────────────────────────────────────────────────

/// <summary>
/// Representação de uma transação retornada pela API.
/// Inclui dados desnormalizados (nomes) para facilitar a exibição no front-end,
/// evitando joins adicionais no cliente.
/// </summary>
public record TransacaoResponseDto(
    Guid Id,
    string Descricao,
    decimal Valor,
    TipoTransacao Tipo,
    Guid CategoriaId,
    string CategoriaNome,
    Guid PessoaId,
    string PessoaNome
);
