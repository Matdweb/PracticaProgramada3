using System.ComponentModel.DataAnnotations;

namespace PracticaProgramada3.Application.Dtos;

public class VotanteDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "La cédula debe tener 9 caracteres.")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
    public string NombreCompleto { get; set; } = string.Empty;
}
