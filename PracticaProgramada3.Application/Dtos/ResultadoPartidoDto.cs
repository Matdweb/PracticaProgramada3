namespace PracticaProgramada3.Application.Dtos;

/// <summary>Un partido con la cantidad de votos que obtuvo (pantalla de resultados).</summary>
public class ResultadoPartidoDto
{
    public int PartidoId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int CantidadVotos { get; set; }
}
