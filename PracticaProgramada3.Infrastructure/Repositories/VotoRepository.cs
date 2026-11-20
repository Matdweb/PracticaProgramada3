using Microsoft.EntityFrameworkCore;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Domain.Entities;
using PracticaProgramada3.Infrastructure.Data;

namespace PracticaProgramada3.Infrastructure.Repositories;

public class VotoRepository : IVotoRepository
{
    private readonly ApplicationDbContext _context;

    public VotoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteVotoDeVotanteAsync(int votanteId, CancellationToken cancellationToken = default)
    {
        return await _context.Votos
            .AsNoTracking()
            .AnyAsync(v => v.VotanteId == votanteId, cancellationToken);
    }

    public async Task<bool> CreateVotoAsync(Voto voto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Votos.AddAsync(voto, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateException)
        {
            // Normalmente: el índice único de VotanteId rechazó un segundo voto de la misma cédula
            _context.Entry(voto).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<List<ResultadoPartidoDto>> GetResultadosAsync(CancellationToken cancellationToken = default)
    {
        // Se parte de Partidos (no de Votos) para que los partidos sin votos aparezcan con 0
        return await _context.Partidos
            .AsNoTracking()
            .Select(p => new ResultadoPartidoDto
            {
                PartidoId = p.Id,
                Nombre = p.Nombre,
                CantidadVotos = _context.Votos.Count(v => v.PartidoId == p.Id)
            })
            .OrderByDescending(r => r.CantidadVotos)
            .ThenBy(r => r.Nombre)
            .ToListAsync(cancellationToken);
    }
}
