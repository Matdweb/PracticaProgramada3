using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Services;

namespace PracticaProgramada3.Api.Controllers;

/// <summary>
/// API de votación. Es un controlador delgado: no tiene reglas de negocio, solo llama al servicio.
/// El código de la Respuesta (200, 400, 404, 409, 500) se devuelve también como código HTTP.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VotacionController : ControllerBase
{
    private readonly IVotacionServicio _votacionServicio;

    public VotacionController(IVotacionServicio votacionServicio)
    {
        _votacionServicio = votacionServicio;
    }

    // GET: api/votacion/verificar/102340567
    [HttpGet("verificar/{cedula}")]
    public async Task<IActionResult> Verificar(string cedula, CancellationToken cancellationToken)
    {
        var respuesta = await _votacionServicio.VerificarVotanteAsync(cedula, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }

    // POST: api/votacion/votar   (cuerpo JSON: { "cedula": "102340567", "partidoId": 1 })
    [HttpPost("votar")]
    public async Task<IActionResult> Votar([FromBody] EmitirVotoDto voto, CancellationToken cancellationToken)
    {
        // [ApiController] ya responde 400 automáticamente si el DTO no cumple sus validaciones
        var respuesta = await _votacionServicio.EmitirVotoAsync(voto, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }

    // GET: api/votacion/resultados
    [HttpGet("resultados")]
    public async Task<IActionResult> Resultados(CancellationToken cancellationToken)
    {
        var resultados = await _votacionServicio.ObtenerResultadosAsync(cancellationToken);
        return Ok(resultados);
    }
}
