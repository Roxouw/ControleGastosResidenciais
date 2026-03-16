using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers;

/// <summary>
/// Controller de pessoas.
/// Expõe as operações de CRUD: listar, buscar, criar, editar e deletar.
/// Toda a lógica de negócio é delegada ao IPessoaService.
/// Erros (404, 422) são tratados pelo ExceptionMiddleware global.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaService _service;

    public PessoasController(IPessoaService service)
    {
        _service = service;
    }

    /// <summary>Retorna a lista de todas as pessoas cadastradas.</summary>
    /// <response code="200">Lista de pessoas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PessoaResponseDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var pessoas = await _service.GetAllAsync();
        return Ok(pessoas);
    }

    /// <summary>Retorna uma pessoa pelo Id.</summary>
    /// <response code="200">Pessoa encontrada.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PessoaResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pessoa = await _service.GetByIdAsync(id);
        return Ok(pessoa);
    }

    /// <summary>Cria uma nova pessoa.</summary>
    /// <response code="201">Pessoa criada com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(PessoaResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreatePessoaDto dto)
    {
        var criada = await _service.CreateAsync(dto);
        // Retorna 201 Created com o header Location apontando para o recurso criado
        return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
    }

    /// <summary>Atualiza os dados de uma pessoa existente.</summary>
    /// <response code="200">Pessoa atualizada com sucesso.</response>
    /// <response code="400">Dados de entrada inválidos.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PessoaResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePessoaDto dto)
    {
        var atualizada = await _service.UpdateAsync(id, dto);
        return Ok(atualizada);
    }

    /// <summary>
    /// Deleta uma pessoa e todas as suas transações (cascade).
    /// </summary>
    /// <response code="204">Pessoa deletada com sucesso.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent(); // 204 — sucesso sem corpo de resposta
    }
}
