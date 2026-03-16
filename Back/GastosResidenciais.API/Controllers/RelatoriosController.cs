using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers;

/// <summary>
/// Controller de relatórios financeiros.
/// Expõe consultas de totais agrupados por pessoa e por categoria,
/// incluindo totais consolidados de todas as entradas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _service;

    public RelatoriosController(IRelatorioService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna totais de receitas, despesas e saldo por pessoa,
    /// seguidos dos totais consolidados de todas as pessoas.
    /// </summary>
    /// <response code="200">Relatório por pessoa com totais gerais.</response>
    [HttpGet("pessoas")]
    [ProducesResponseType(typeof(RelatorioResumoPessoasDto), 200)]
    public async Task<IActionResult> GetTotaisPorPessoa()
    {
        var relatorio = await _service.GetTotaisPorPessoaAsync();
        return Ok(relatorio);
    }

    /// <summary>
    /// Retorna totais de receitas, despesas e saldo por categoria,
    /// seguidos dos totais consolidados de todas as categorias.
    /// </summary>
    /// <response code="200">Relatório por categoria com totais gerais.</response>
    [HttpGet("categorias")]
    [ProducesResponseType(typeof(RelatorioResumoCategoriaDto), 200)]
    public async Task<IActionResult> GetTotaisPorCategoria()
    {
        var relatorio = await _service.GetTotaisPorCategoriaAsync();
        return Ok(relatorio);
    }
}
