using PracticaProgramada3.Application.Dtos;

namespace PracticaProgramada3.Application.Services;

public interface IVotacionServicio
{
    /// <summary>
    /// Verifica que la cédula esté inscrita y todavía no haya votado.
    /// Códigos: 400 formato inválido, 404 no inscrita, 409 ya votó.
    /// </summary>
    Task<Respuesta<VotanteDto>> VerificarVotanteAsync(string cedula, CancellationToken cancellationToken = default);

    /// <summary>
    /// Registra el voto de una cédula por un partido. Aplica las mismas validaciones que la verificación
    /// y además comprueba que el partido exista. Devuelve el partido elegido.
    /// </summary>
    Task<Respuesta<PartidoDto>> EmitirVotoAsync(EmitirVotoDto voto, CancellationToken cancellationToken = default);

    /// <summary>Devuelve cada partido con su cantidad de votos.</summary>
    Task<List<ResultadoPartidoDto>> ObtenerResultadosAsync(CancellationToken cancellationToken = default);
}
