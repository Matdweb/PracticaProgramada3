using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Services;

namespace PracticaProgramada3.Api.Controllers;

/// <summary>
/// API del CRUD de partidos políticos. Es un controlador delgado: no tiene reglas de negocio, solo llama al servicio.
/// El código de la Respuesta (200, 400, 404, 409, 500) se devuelve también como código HTTP.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PartidosController : ControllerBase
{
    private readonly IPartidoServicio _partidoServicio;

    public PartidosController(IPartidoServicio partidoServicio)
    {
        _partidoServicio = partidoServicio;
    }

    // GET: api/partidos
    [HttpGet]
    public async Task<IActionResult> ObtenerPartidos(CancellationToken cancellationToken)
    {
        var partidos = await _partidoServicio.ObtenerPartidosAsync(cancellationToken);
        return Ok(partidos);
    }

    // GET: api/partidos/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPartidoPorId(int id, CancellationToken cancellationToken)
    {
        var partido = await _partidoServicio.ObtenerPartidoPorIdAsync(id, cancellationToken);
        if (partido is null)
        {
            return NotFound(new Respuesta<PartidoDto>
            {
                EsCorrecto = false,
                Mensaje = "El partido no existe.",
                Codigo = 404
            });
        }

        return Ok(partido);
    }

    // POST: api/partidos   (cuerpo JSON: { "nombre": "Partido Azul" })
    [HttpPost]
    public async Task<IActionResult> CrearPartido([FromBody] PartidoDto partido, CancellationToken cancellationToken)
    {
        var respuesta = await _partidoServicio.CrearPartidoAsync(partido, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }

    // POST: api/partidos/actualizar   (cuerpo JSON: { "id": 1, "nombre": "Partido Azul" })
    [HttpPost("actualizar")]
    public async Task<IActionResult> ActualizarPartido([FromBody] PartidoDto partido, CancellationToken cancellationToken)
    {
        var respuesta = await _partidoServicio.ActualizarPartidoAsync(partido, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }

    // DELETE: api/partidos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarPartido(int id, CancellationToken cancellationToken)
    {
        var respuesta = await _partidoServicio.EliminarPartidoAsync(id, cancellationToken);
        return StatusCode(respuesta.Codigo, respuesta);
    }
}
