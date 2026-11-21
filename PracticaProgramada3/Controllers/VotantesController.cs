using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Services;

namespace PracticaProgramada3.Controllers;

[AutoValidateAntiforgeryToken]
public class VotantesController : Controller
{
    private readonly IVotacionApiClient _votacionApiClient;

    public VotantesController(IVotacionApiClient votacionApiClient)
    {
        _votacionApiClient = votacionApiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerVotantes(CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.ObtenerVotantesAsync(cancellationToken);
        return Json(respuesta);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerVotantePorId(int id, CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.ObtenerVotantePorIdAsync(id, cancellationToken);
        return Json(respuesta);
    }

    [HttpPost]
    public async Task<IActionResult> CrearVotante(VotanteDto votante, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var respuesta = await _votacionApiClient.CrearVotanteAsync(votante, cancellationToken);
        return Json(respuesta);
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarVotante(VotanteDto votante, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var respuesta = await _votacionApiClient.ActualizarVotanteAsync(votante, cancellationToken);
        return Json(respuesta);
    }

    [HttpPost]
    public async Task<IActionResult> EliminarVotante(int id, CancellationToken cancellationToken)
    {
        var respuesta = await _votacionApiClient.EliminarVotanteAsync(id, cancellationToken);
        return Json(respuesta);
    }
}
