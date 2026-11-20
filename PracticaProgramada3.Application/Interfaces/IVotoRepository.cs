using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Interfaces;

public interface IVotoRepository
{
    /// <summary>Indica si el votante ya tiene un voto registrado.</summary>
    Task<bool> ExisteVotoDeVotanteAsync(int votanteId, CancellationToken cancellationToken = default);

    /// <summary>Guarda el voto. Devuelve false si no se pudo guardar (por ejemplo, el votante ya tenía un voto).</summary>
    Task<bool> CreateVotoAsync(Voto voto, CancellationToken cancellationToken = default);

    /// <summary>Cantidad de votos de cada partido (incluye los partidos con 0 votos), de mayor a menor.</summary>
    Task<List<ResultadoPartidoDto>> GetResultadosAsync(CancellationToken cancellationToken = default);
}
