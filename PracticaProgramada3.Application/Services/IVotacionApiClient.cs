using PracticaProgramada3.Application.Dtos;

namespace PracticaProgramada3.Services;

/// <summary>
/// Cliente que usa el proyecto MVC para consumir la API REST de votación.
/// El MVC nunca habla con la base de datos: todo pasa por estos métodos.
/// </summary>
public interface IVotacionApiClient
{
    /// <summary>Llama a GET api/votacion/verificar/{cedula}.</summary>
    Task<Respuesta<VotanteDto>> VerificarVotanteAsync(string cedula, CancellationToken cancellationToken = default);

    /// <summary>Llama a GET api/partidos.</summary>
    Task<Respuesta<List<PartidoDto>>> ObtenerPartidosAsync(CancellationToken cancellationToken = default);

    /// <summary>Llama a POST api/votacion/votar.</summary>
    Task<Respuesta<PartidoDto>> EmitirVotoAsync(EmitirVotoDto voto, CancellationToken cancellationToken = default);

    /// <summary>Llama a GET api/votacion/resultados.</summary>
    Task<Respuesta<List<ResultadoPartidoDto>>> ObtenerResultadosAsync(CancellationToken cancellationToken = default);
}
