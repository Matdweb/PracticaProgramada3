using System.ComponentModel.DataAnnotations;

namespace PracticaProgramada3.Application.Dtos;

public class PartidoDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del partido es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}
