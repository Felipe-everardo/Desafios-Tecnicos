using Microsoft.AspNetCore.Mvc;
using TechHelpSystem.Api.DTO;
using TechHelpSystem.Api.Services;

[Route("api/Chamados")]
[ApiController]
public class ChamadosController : ControllerBase
{
    private readonly ChamadoService _chamadoService;

    public ChamadosController(ChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChamadoResponse>> CreatChamadoAsync(
            [FromBody] ChamadoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var chamadoCriado = await _chamadoService.CreateChamadoAsync(request, cancellationToken);

            var resposta = CreatedAtAction(nameof(GetChamadoById), new { id = chamadoCriado.Id }, chamadoCriado);
            return resposta;
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<ChamadoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ChamadoResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChamadoResponse>> GetChamadoById(int id, CancellationToken cancellationToken)
    {
        var chamado = await _chamadoService.GetChamadoByIdAsync(id, cancellationToken);

        if (chamado is null)
        {
            return NotFound();
        }

        return Ok(chamado);
    }
}