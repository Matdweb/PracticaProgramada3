using Microsoft.EntityFrameworkCore;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Domain.Entities;
using PracticaProgramada3.Infrastructure.Data;

namespace PracticaProgramada3.Infrastructure.Repositories;

public class PartidoRepository : IPartidoRepository
{
    private readonly ApplicationDbContext _context;

    public PartidoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Partido>> GetAllPartidosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Partidos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Partido> GetPartidoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var partido = await _context.Partidos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return partido ?? new Partido();
    }

    public async Task<Partido> GetPartidoByNombreAsync(string nombre, CancellationToken cancellationToken = default)
    {
        var partido = await _context.Partidos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nombre == nombre, cancellationToken);

        return partido ?? new Partido();
    }

    public async Task<bool> CreateAsync(Partido partido, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Partidos.AddAsync(partido, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateException)
        {
            // Normalmente: el índice único de Nombre rechazó un partido repetido
            _context.Entry(partido).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Partido partido, CancellationToken cancellationToken = default)
    {
        try
        {
            var existente = await _context.Partidos.FindAsync([partido.Id], cancellationToken);
            if (existente is null)
            {
                return false;
            }

            existente.Nombre = partido.Nombre;

            // Si el nombre no cambió, SaveChanges devuelve 0 pero la operación es correcta
            if (!_context.ChangeTracker.HasChanges())
            {
                return true;
            }

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
            var partido = await _context.Partidos.FindAsync([id], cancellationToken);
            if (partido is null)
            {
                return false;
            }

            _context.Partidos.Remove(partido);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateException)
        {
            // La llave foránea con DeleteBehavior.Restrict no deja borrar un partido que ya tiene votos
            _context.ChangeTracker.Clear();
            return false;
        }
    }
}
