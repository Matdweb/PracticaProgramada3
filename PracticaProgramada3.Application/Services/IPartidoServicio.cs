using PracticaProgramada3.Application.Dtos;

namespace PracticaProgramada3.Application.Services;

public interface IPartidoServicio
{
    /// <summary>Lista los partidos por los que se puede votar.</summary>
    Task<List<PartidoDto>> ObtenerPartidosAsync(CancellationToken cancellationToken = default);
}
