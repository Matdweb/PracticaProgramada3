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
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Cedula == cedula, cancellationToken);

        return votante ?? new Votante();
    }

    public async Task<List<Votante>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Votantes
            .AsNoTracking()
            .OrderBy(v => v.NombreCompleto)
            .ToListAsync(cancellationToken);
    }

    public async Task<Votante?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var votante = await _context.Votantes
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        return votante;
    }

    public async Task<bool> CreateAsync(Votante votante, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Votantes.AddAsync(votante, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateException)
        {
            _context.Entry(votante).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Votante votante, CancellationToken cancellationToken = default)
    {
        try
        {
            var existente = await _context.Votantes.FindAsync([votante.Id], cancellationToken);
            if (existente is null)
            {
                return false;
            }

            existente.Cedula = votante.Cedula;
            existente.NombreCompleto = votante.NombreCompleto;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateException)
        {
            _context.ChangeTracker.Clear();
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var votante = await _context.Votantes.FindAsync([id], cancellationToken);
            if (votante is null)
            {
                return false;
            }

            _context.Votantes.Remove(votante);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateException)
        {
            _context.ChangeTracker.Clear();
            return false;
        }
    }
}
