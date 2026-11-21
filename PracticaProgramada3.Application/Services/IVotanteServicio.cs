using PracticaProgramada3.Application.Dtos;

namespace PracticaProgramada3.Application.Services;

public interface IVotanteServicio
{
    Task<List<VotanteDto>> ObtenerVotantesAsync(CancellationToken cancellationToken = default);
    Task<VotanteDto?> ObtenerVotantePorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> CrearVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> ActualizarVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default);
    Task<Respuesta<VotanteDto>> EliminarVotanteAsync(int id, CancellationToken cancellationToken = default);
}
