using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Interfaces;

public interface IVotanteRepository
{

    Task<Votante> GetVotanteByCedulaAsync(string cedula, CancellationToken cancellationToken = default);

    Task<List<Votante>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Votante?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(Votante votante, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Votante votante, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
