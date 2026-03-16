namespace GastosResidenciais.Application.DTOs;

/// <summary>
/// Totais financeiros de uma entidade (pessoa ou categoria).
/// Contém receitas, despesas e o saldo calculado (receita - despesa).
/// </summary>
public record TotaisDto(
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo          // Saldo = Receitas - Despesas
);

/// <summary>
/// Linha do relatório de totais por pessoa.
/// Agrupa os dados da pessoa com seus totais financeiros.
/// </summary>
public record RelatorioPessoaDto(
    Guid PessoaId,
    string PessoaNome,
    int PessoaIdade,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo
);

/// <summary>
/// Linha do relatório de totais por categoria.
/// Agrupa os dados da categoria com seus totais financeiros.
/// </summary>
public record RelatorioCategoriaDto(
    Guid CategoriaId,
    string CategoriaDescricao,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo
);

/// <summary>
/// Resposta completa do relatório por pessoa.
/// Contém a lista de pessoas com seus totais e o consolidado geral.
/// </summary>
public record RelatorioResumoPessoasDto(
    IEnumerable<RelatorioPessoaDto> Pessoas,
    TotaisDto TotaisGerais
);

/// <summary>
/// Resposta completa do relatório por categoria.
/// Contém a lista de categorias com seus totais e o consolidado geral.
/// </summary>
public record RelatorioResumoCategoriaDto(
    IEnumerable<RelatorioCategoriaDto> Categorias,
    TotaisDto TotaisGerais
);
