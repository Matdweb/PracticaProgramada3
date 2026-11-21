namespace PracticaProgramada3.Domain.Entities;

public class Votante
{
    public int Id { get; set; }

    public string Cedula { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public bool ValidarReglaCedula()
    {
        return Cedula.Length == 9 && Cedula.All(c => c is >= '0' and <= '9');
    }

    public bool ValidarReglaPuedeVotar(bool yaVoto)
    {
        return !yaVoto;
    }
}
