using Microsoft.AspNetCore.Mvc;
using PracticaProgramada3.Application.Services;

namespace PracticaProgramada3.Api.Controllers;

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
}
