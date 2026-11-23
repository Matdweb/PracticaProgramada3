namespace PracticaProgramada3.Domain.Entities;

/// <summary>Partido político por el que se puede votar.</summary>
public class Partido
{
    public int Id { get; set; }

    /// <summary>Nombre del partido. Es único.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Regla de negocio: el nombre es obligatorio y debe tener entre 2 y 100 caracteres.</summary>
    public bool ValidarReglaNombre()
    {
        return !string.IsNullOrWhiteSpace(Nombre) && Nombre.Length is >= 2 and <= 100;
    }
}
