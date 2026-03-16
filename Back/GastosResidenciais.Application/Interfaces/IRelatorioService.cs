using GastosResidenciais.Application.DTOs;

namespace GastosResidenciais.Application.Interfaces;

/// <summary>
/// Contrato de serviço para geração de relatórios de totais financeiros.
/// </summary>
public interface IRelatorioService
{
    /// <summary>
    /// Retorna o relatório de totais agrupado por pessoa.
    /// Para cada pessoa: total de receitas, total de despesas e saldo.
    /// Ao final, retorna também os totais consolidados de todas as pessoas.
    /// </summary>
    Task<RelatorioResumoPessoasDto> GetTotaisPorPessoaAsync();

    /// <summary>
    /// Retorna o relatório de totais agrupado por categoria.
    /// Para cada categoria: total de receitas, total de despesas e saldo.
    /// Ao final, retorna também os totais consolidados de todas as categorias.
    /// </summary>
    Task<RelatorioResumoCategoriaDto> GetTotaisPorCategoriaAsync();
}
