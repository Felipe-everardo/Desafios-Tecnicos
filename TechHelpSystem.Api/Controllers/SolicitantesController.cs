using Microsoft.AspNetCore.Mvc;
using TechHelpSystem.Api.DTO;
using TechHelpSystem.Api.Exceptions;
using TechHelpSystem.Api.Services;

namespace TechHelpSystem.Api.Controllers;

[Route("api/Solicitantes")]
[ApiController]
public class SolicitantesController : ControllerBase
{
    private readonly SolicitanteService _solicitanteService;

    public SolicitantesController(SolicitanteService solicitanteService)
    {
        _solicitanteService = solicitanteService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SolicitanteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SolicitanteResponse>>> Get(CancellationToken cancellationToken)
    {
        var solicitantes = await _solicitanteService.GetAllSolicitantesAsync(cancellationToken);

        return Ok(solicitantes);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SolicitanteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SolicitanteResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var solicitante = await _solicitanteService.GetSolicitanteByIdAsync(id, cancellationToken);

        if (solicitante is null)
        {
            return NotFound();
        }

        return Ok(solicitante);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SolicitanteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SolicitanteResponse>> Post(
        [FromBody] SolicitanteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var solicitante = await _solicitanteService.CreateSolicitanteAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = solicitante.Id }, solicitante);
        }
        catch (EmailDuplicadoException e)
        {
            return Conflict(new ValidationProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Email Duplicado",
                Detail = e.Message
            });
        }
    }
}

