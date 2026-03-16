using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using GastosResidenciais.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers;

/// <summary>
/// Controller de categorias.
/// Expõe listagem completa, listagem filtrada por tipo e criação.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    /// <summary>Retorna todas as categorias cadastradas.</summary>
    /// <response code="200">Lista de categorias.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponseDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _service.GetAllAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Retorna apenas as categorias compatíveis com o tipo de transação informado.
    /// Utilizado pelo front-end para popular o select de categorias
    /// conforme o tipo escolhido pelo usuário.
    /// Ex.: GET /api/categorias/compatíveis?tipo=0 → categorias de Despesa e Ambas.
    /// </summary>
    /// <param name="tipo">0=Despesa, 1=Receita</param>
    /// <response code="200">Lista de categorias compatíveis.</response>
    [HttpGet("compativeis")]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponseDto>), 200)]
    public async Task<IActionResult> GetCompatible([FromQuery] TipoTransacao tipo)
    {
        var categorias = await _service.GetCompatibleAsync(tipo);
        return Ok(categorias);
    }

    /// <summary>Cria uma nova categoria.</summary>
    /// <response code="201">Categoria criada com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateCategoriaDto dto)
    {
        var criada = await _service.CreateAsync(dto);
        return StatusCode(201, criada);
    }
}
