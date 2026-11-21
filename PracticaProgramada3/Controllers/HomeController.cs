using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Models;
using PracticaProgramada3.Services;

namespace PracticaProgramada3.Controllers;

/// <summary>
/// Pantalla de votación. No tiene reglas de negocio: recibe lo que envía el navegador (AJAX),
/// lo manda a la API por medio de IVotacionApiClient y devuelve la Respuesta como JSON.
/// </summary>
[AutoValidateAntiforgeryToken] // Protege las acciones POST contra CSRF
public class HomeController : Controller
{
    private readonly IVotacionApiClient _votacionApiClient;

    public HomeController(IVotacionApiClient votacionApiClient)
    {
        _votacionApiClient = votacionApiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Paso 1: la cédula debe estar inscrita y no haber votado
    [HttpPost]
    public async Task<IActionResult> Verificar(string cedula, CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.VerificarVotanteAsync(cedula ?? string.Empty, cancellationToken);
        return Json(respuesta);
    }

    // Paso 2: lista de partidos para elegir
    public async Task<IActionResult> ObtenerPartidos(CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.ObtenerPartidosAsync(cancellationToken);
        return Json(respuesta);
    }

    // Paso 3: registrar el voto
    [HttpPost]
    public async Task<IActionResult> Votar(EmitirVotoDto voto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var respuesta = await _votacionApiClient.EmitirVotoAsync(voto, cancellationToken);
        return Json(respuesta);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
