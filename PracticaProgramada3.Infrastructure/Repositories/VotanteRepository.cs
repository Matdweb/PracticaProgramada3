using Microsoft.EntityFrameworkCore;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Domain.Entities;
using PracticaProgramada3.Infrastructure.Data;

namespace PracticaProgramada3.Infrastructure.Repositories;

public class VotanteRepository : IVotanteRepository
{
    private readonly ApplicationDbContext _context;

    public VotanteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Votante> GetVotanteByCedulaAsync(string cedula, CancellationToken cancellationToken = default)
    {
        var votante = await _context.Votantes
            .AsNoTracking() // solo lectura: no necesita seguimiento de cambios
            .FirstOrDefaultAsync(v => v.Cedula == cedula, cancellationToken);

        return votante ?? new Votante();
    }
}
