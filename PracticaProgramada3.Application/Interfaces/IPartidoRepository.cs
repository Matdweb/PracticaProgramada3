using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Interfaces;

public interface IPartidoRepository
{
    Task<List<Partido>> GetAllPartidosAsync(CancellationToken cancellationToken = default);

    /// <summary>Busca un partido por Id. Si no existe devuelve un objeto vacío (Id = 0).</summary>
    Task<Partido> GetPartidoByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Busca un partido por nombre. Si no existe devuelve un objeto vacío (Id = 0).</summary>
    Task<Partido> GetPartidoByNombreAsync(string nombre, CancellationToken cancellationToken = default);

    Task<bool> CreateAsync(Partido partido, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Partido partido, CancellationToken cancellationToken = default);

    /// <summary>Elimina el partido. Devuelve false si no se pudo (por ejemplo, ya tiene votos registrados).</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
