namespace PracticaProgramada3.Domain.Entities;

/// <summary>
/// Persona inscrita en el padrón para votar. Se identifica solo con su cédula (no hay contraseña).
/// Las cédulas ya existen en la base de datos: el sistema no las crea al votar.
/// </summary>
public class Votante
{
    public int Id { get; set; }

    /// <summary>Número de cédula, solo dígitos. Es único.</summary>
    public string Cedula { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>Regla de negocio: la cédula debe tener exactamente 9 dígitos numéricos.</summary>
    /// <returns>true si el formato de la cédula es válido.</returns>
    public bool ValidarReglaCedula()
    {
        return Cedula.Length == 9 && Cedula.All(c => c is >= '0' and <= '9');
    }

    /// <summary>Regla de negocio: cada cédula puede votar una sola vez.</summary>
    /// <param name="yaVoto">true si la base de datos ya tiene un voto de este votante.</param>
    /// <returns>true si todavía puede votar.</returns>
    public bool ValidarReglaPuedeVotar(bool yaVoto)
    {
        return !yaVoto;
    }
}
