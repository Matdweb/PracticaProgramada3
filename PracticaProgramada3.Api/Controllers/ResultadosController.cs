using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Services;

namespace PracticaProgramada3.Controllers;

/// <summary>Pantalla que muestra los partidos y la cantidad de votos de cada uno.</summary>
public class ResultadosController : Controller
{
    private readonly IVotacionApiClient _votacionApiClient;

    public ResultadosController(IVotacionApiClient votacionApiClient)
    {
        _votacionApiClient = votacionApiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Retorna los resultados en formato JSON (no una vista)
    public async Task<IActionResult> Obtener(CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.ObtenerResultadosAsync(cancellationToken);
        return Json(respuesta);
    }
}
