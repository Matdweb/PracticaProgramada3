namespace PracticaProgramada3.Domain.Entities;

/// <summary>Partido político por el que se puede votar.</summary>
public class Partido
{
    public int Id { get; set; }

    /// <summary>Nombre del partido. Es único.</summary>
    public string Nombre { get; set; } = string.Empty;
}
