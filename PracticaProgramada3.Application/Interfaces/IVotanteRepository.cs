using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Interfaces;

public interface IVotanteRepository
{
    /// <summary>Busca un votante por su cédula. Si no existe devuelve un objeto vacío (Id = 0).</summary>
    Task<Votante> GetVotanteByCedulaAsync(string cedula, CancellationToken cancellationToken = default);
}
