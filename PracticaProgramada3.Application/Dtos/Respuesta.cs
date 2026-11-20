namespace PracticaProgramada3.Application.Dtos;

/// <summary>
/// Resultado de una operación de negocio (patrón Result): evita lanzar excepciones por reglas de negocio.
/// Códigos usados: 200 ok, 400 validación, 404 no existe, 409 conflicto (ya votó), 500 error al guardar.
/// </summary>
public class Respuesta<T>
{
    public bool EsCorrecto { get; set; } = true;

    public string Mensaje { get; set; } = string.Empty;

    public int Codigo { get; set; } = 200;

    public T? Dato { get; set; }
}
