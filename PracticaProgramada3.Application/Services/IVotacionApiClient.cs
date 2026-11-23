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

    Task<Respuesta<List<VotanteDto>>> ObtenerVotantesAsync(CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> ObtenerVotantePorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> CrearVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> ActualizarVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> EliminarVotanteAsync(int id, CancellationToken cancellationToken = default);

    // CRUD de partidos (la lista completa se obtiene con ObtenerPartidosAsync)
    Task<Respuesta<PartidoDto>> ObtenerPartidoPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Respuesta<PartidoDto>> CrearPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default);
    Task<Respuesta<PartidoDto>> ActualizarPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default);
    Task<Respuesta<PartidoDto>> EliminarPartidoAsync(int id, CancellationToken cancellationToken = default);
}
