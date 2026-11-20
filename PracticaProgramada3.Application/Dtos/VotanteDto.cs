namespace PracticaProgramada3.Application.Dtos;

/// <summary>Datos de un votante que se muestran al verificar su cédula.</summary>
public class VotanteDto
{
    public int Id { get; set; }

    public string Cedula { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;
}
