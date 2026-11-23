using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Services;

namespace PracticaProgramada3.Controllers;

/// <summary>
/// Pantalla de administración de partidos políticos. No tiene reglas de negocio: recibe lo que envía
/// el navegador (AJAX), lo manda a la API por medio de IVotacionApiClient y devuelve la Respuesta como JSON.
/// </summary>
[AutoValidateAntiforgeryToken] // Protege las acciones POST contra CSRF
public class PartidosController : Controller
{
    private readonly IVotacionApiClient _votacionApiClient;

    public PartidosController(IVotacionApiClient votacionApiClient) // Inyección de dependencias
    {
        _votacionApiClient = votacionApiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPartidos(CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.ObtenerPartidosAsync(cancellationToken);
        return Json(respuesta);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPartidoPorId(int id, CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.ObtenerPartidoPorIdAsync(id, cancellationToken);
        return Json(respuesta);
    }

    [HttpPost]
    public async Task<IActionResult> CrearPartido(PartidoDto partido, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var respuesta = await _votacionApiClient.CrearPartidoAsync(partido, cancellationToken);
        return Json(respuesta);
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarPartido(PartidoDto partido, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var respuesta = await _votacionApiClient.ActualizarPartidoAsync(partido, cancellationToken);
        return Json(respuesta);
    }

    [HttpPost]
    public async Task<IActionResult> EliminarPartido(int id, CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.EliminarPartidoAsync(id, cancellationToken);
        return Json(respuesta);
    }
}
