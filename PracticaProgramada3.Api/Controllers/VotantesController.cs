using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Services;

namespace PracticaProgramada3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VotantesController : ControllerBase
{
    private readonly IVotanteServicio _votanteServicio;

    public VotantesController(IVotanteServicio votanteServicio)
    {
        _votanteServicio = votanteServicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerVotantes(CancellationToken cancellationToken)
    {
        var votantes = await _votanteServicio.ObtenerVotantesAsync(cancellationToken);
        return Ok(votantes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerVotantePorId(int id, CancellationToken cancellationToken)
    {
        var votante = await _votanteServicio.ObtenerVotantePorIdAsync(id, cancellationToken);
        if (votante is null)
        {
            return NotFound(new Respuesta<VotanteDto>
            {
                EsCorrecto = false,
                Mensaje = "El votante no existe.",
                Codigo = 404
            });
        }

        return Ok(votante);
    }

    [HttpPost]
    public async Task<IActionResult> CrearVotante([FromBody] VotanteDto votante, CancellationToken cancellationToken)
    {
        var respuesta = await _votanteServicio.CrearVotanteAsync(votante, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }

    [HttpPost("actualizar")]
    public async Task<IActionResult> ActualizarVotante([FromBody] VotanteDto votante, CancellationToken cancellationToken)
    {
        var respuesta = await _votanteServicio.ActualizarVotanteAsync(votante, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarVotante(int id, CancellationToken cancellationToken)
    {
        var respuesta = await _votanteServicio.EliminarVotanteAsync(id, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }
}
