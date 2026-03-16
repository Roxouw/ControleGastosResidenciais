using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers;

/// <summary>
/// Controller de transações financeiras.
/// Expõe listagem e criação. As regras de negócio são aplicadas
/// na camada de Application (TransacaoService).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _service;

    public TransacoesController(ITransacaoService service)
    {
        _service = service;
    }

    /// <summary>Retorna todas as transações com dados de pessoa e categoria.</summary>
    /// <response code="200">Lista de transações.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransacaoResponseDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var transacoes = await _service.GetAllAsync();
        return Ok(transacoes);
    }

    /// <summary>
    /// Cria uma nova transação aplicando as regras de negócio:
    /// - Menor de 18 anos: apenas Despesas são aceitas.
    /// - Categoria deve ser compatível com o tipo da transação.
    /// </summary>
    /// <response code="201">Transação criada com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos (validação de estrutura).</response>
    /// <response code="404">Pessoa ou categoria não encontrada.</response>
    /// <response code="422">Violação de regra de negócio.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TransacaoResponseDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> Create([FromBody] CreateTransacaoDto dto)
    {
        var criada = await _service.CreateAsync(dto);
        return StatusCode(201, criada);
    }
}
