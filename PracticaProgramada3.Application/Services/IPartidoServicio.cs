using PracticaProgramada3.Application.Dtos;

namespace PracticaProgramada3.Application.Services;

public interface IPartidoServicio
{
    /// <summary>Lista los partidos por los que se puede votar.</summary>
    Task<List<PartidoDto>> ObtenerPartidosAsync(CancellationToken cancellationToken = default);

    Task<PartidoDto?> ObtenerPartidoPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Respuesta<PartidoDto>> CrearPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default);
    Task<Respuesta<PartidoDto>> ActualizarPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default);
    Task<Respuesta<PartidoDto>> EliminarPartidoAsync(int id, CancellationToken cancellationToken = default);
}
