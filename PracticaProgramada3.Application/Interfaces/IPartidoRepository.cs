using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Interfaces;

public interface IPartidoRepository
{
    Task<List<Partido>> GetAllPartidosAsync(CancellationToken cancellationToken = default);

    /// <summary>Busca un partido por Id. Si no existe devuelve un objeto vacío (Id = 0).</summary>
    Task<Partido> GetPartidoByIdAsync(int id, CancellationToken cancellationToken = default);
}
