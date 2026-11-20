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
}
