namespace PracticaProgramada3.Domain.Entities;

/// <summary>Voto emitido por un votante a favor de un partido. Cada votante tiene como máximo un voto.</summary>
public class Voto
{
    public int Id { get; set; }

    public int VotanteId { get; set; }

    public int PartidoId { get; set; }

    public DateTime FechaVoto { get; set; } = DateTime.UtcNow;
}
