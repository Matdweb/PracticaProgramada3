using System.ComponentModel.DataAnnotations;

namespace PracticaProgramada3.Application.Dtos;

/// <summary>Datos que se envían para registrar un voto: la cédula de quien vota y el partido elegido.</summary>
public class EmitirVotoDto
{
    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [RegularExpression(@"^[\d\s-]{9,15}$", ErrorMessage = "Ingresa una cédula válida (9 dígitos).")]
    public string Cedula { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un partido.")]
    public int PartidoId { get; set; }
}
