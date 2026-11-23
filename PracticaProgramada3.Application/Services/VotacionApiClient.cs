using System.Net.Http.Json;
using System.Text.Json;
using PracticaProgramada3.Application.Dtos;

namespace PracticaProgramada3.Services;

public class VotacionApiClient : IVotacionApiClient
{
    private const string MensajeSinConexion = "No se pudo conectar con el servicio de votación. Intenta de nuevo.";

    // Web = nombres en camelCase y sin distinguir mayúsculas, igual que responde la API
    private static readonly JsonSerializerOptions OpcionesJson = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;

    public VotacionApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Respuesta<VotanteDto>> VerificarVotanteAsync(string cedula, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.GetAsync(
                $"api/votacion/verificar/{Uri.EscapeDataString(cedula)}", cancellationToken);
            return await LeerRespuestaAsync<VotanteDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<List<PartidoDto>>> ObtenerPartidosAsync(CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.GetAsync("api/partidos", cancellationToken);
            if (!respuestaHttp.IsSuccessStatusCode)
            {
                return CrearError<List<PartidoDto>>(500, "No se pudieron cargar los partidos.");
            }

            var partidos = await respuestaHttp.Content.ReadFromJsonAsync<List<PartidoDto>>(OpcionesJson, cancellationToken);
            return new Respuesta<List<PartidoDto>> { Dato = partidos ?? new List<PartidoDto>() };
        }, cancellationToken);
    }

    public Task<Respuesta<PartidoDto>> EmitirVotoAsync(EmitirVotoDto voto, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.PostAsJsonAsync("api/votacion/votar", voto, OpcionesJson, cancellationToken);
            return await LeerRespuestaAsync<PartidoDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<List<ResultadoPartidoDto>>> ObtenerResultadosAsync(CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.GetAsync("api/votacion/resultados", cancellationToken);
            if (!respuestaHttp.IsSuccessStatusCode)
            {
                return CrearError<List<ResultadoPartidoDto>>(500, "No se pudieron cargar los resultados.");
            }

            var resultados = await respuestaHttp.Content.ReadFromJsonAsync<List<ResultadoPartidoDto>>(OpcionesJson, cancellationToken);
            return new Respuesta<List<ResultadoPartidoDto>> { Dato = resultados ?? new List<ResultadoPartidoDto>() };
        }, cancellationToken);
    }

    public Task<Respuesta<List<VotanteDto>>> ObtenerVotantesAsync(CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.GetAsync("api/votantes", cancellationToken);
            if (!respuestaHttp.IsSuccessStatusCode)
            {
                return CrearError<List<VotanteDto>>(500, "No se pudieron cargar los votantes.");
            }

            var votantes = await respuestaHttp.Content.ReadFromJsonAsync<List<VotanteDto>>(OpcionesJson, cancellationToken);
            return new Respuesta<List<VotanteDto>> { Dato = votantes ?? new List<VotanteDto>() };
        }, cancellationToken);
    }

    public Task<Respuesta<VotanteDto>> ObtenerVotantePorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.GetAsync($"api/votantes/{id}", cancellationToken);
            if (!respuestaHttp.IsSuccessStatusCode)
            {
                return await LeerRespuestaAsync<VotanteDto>(respuestaHttp, cancellationToken);
            }

            var votante = await respuestaHttp.Content.ReadFromJsonAsync<VotanteDto>(OpcionesJson, cancellationToken);
            return votante is null
                ? CrearError<VotanteDto>(404, "El votante no existe.")
                : new Respuesta<VotanteDto> { Dato = votante };
        }, cancellationToken);
    }

    public Task<Respuesta<VotanteDto>> CrearVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.PostAsJsonAsync("api/votantes", votante, OpcionesJson, cancellationToken);
            return await LeerRespuestaAsync<VotanteDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<VotanteDto>> ActualizarVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.PostAsJsonAsync("api/votantes/actualizar", votante, OpcionesJson, cancellationToken);
            return await LeerRespuestaAsync<VotanteDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<VotanteDto>> EliminarVotanteAsync(int id, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.DeleteAsync($"api/votantes/{id}", cancellationToken);
            return await LeerRespuestaAsync<VotanteDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<PartidoDto>> ObtenerPartidoPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.GetAsync($"api/partidos/{id}", cancellationToken);
            if (!respuestaHttp.IsSuccessStatusCode)
            {
                return await LeerRespuestaAsync<PartidoDto>(respuestaHttp, cancellationToken);
            }

            var partido = await respuestaHttp.Content.ReadFromJsonAsync<PartidoDto>(OpcionesJson, cancellationToken);
            return partido is null
                ? CrearError<PartidoDto>(404, "El partido no existe.")
                : new Respuesta<PartidoDto> { Dato = partido };
        }, cancellationToken);
    }

    public Task<Respuesta<PartidoDto>> CrearPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.PostAsJsonAsync("api/partidos", partido, OpcionesJson, cancellationToken);
            return await LeerRespuestaAsync<PartidoDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<PartidoDto>> ActualizarPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.PostAsJsonAsync("api/partidos/actualizar", partido, OpcionesJson, cancellationToken);
            return await LeerRespuestaAsync<PartidoDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    public Task<Respuesta<PartidoDto>> EliminarPartidoAsync(int id, CancellationToken cancellationToken = default)
    {
        return EjecutarAsync(async () =>
        {
            using var respuestaHttp = await _httpClient.DeleteAsync($"api/partidos/{id}", cancellationToken);
            return await LeerRespuestaAsync<PartidoDto>(respuestaHttp, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Ejecuta una llamada a la API y, si no hay conexión o se agota el tiempo, devuelve una Respuesta de error (503)
    /// en lugar de dejar que la excepción llegue a la pantalla.
    /// </summary>
    private static async Task<Respuesta<T>> EjecutarAsync<T>(Func<Task<Respuesta<T>>> operacion, CancellationToken cancellationToken)
    {
        try
        {
            return await operacion();
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested
                                   && (ex is HttpRequestException || ex is TaskCanceledException))
        {
            return CrearError<T>(503, MensajeSinConexion);
        }
    }

    /// <summary>
    /// Lee la Respuesta que devuelve la API (también cuando el código HTTP es 400, 404, 409 o 500).
    /// Si el cuerpo no es una Respuesta (por ejemplo, el 400 automático de [ApiController]), arma un error.
    /// </summary>
    private static async Task<Respuesta<T>> LeerRespuestaAsync<T>(HttpResponseMessage respuestaHttp, CancellationToken cancellationToken)
    {
        var contenido = await respuestaHttp.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            using var documento = JsonDocument.Parse(contenido);

            if (documento.RootElement.ValueKind == JsonValueKind.Object
                && documento.RootElement.TryGetProperty("esCorrecto", out _))
            {
                var respuesta = JsonSerializer.Deserialize<Respuesta<T>>(contenido, OpcionesJson);
                if (respuesta is not null)
                {
                    return respuesta;
                }
            }
        }
        catch (JsonException)
        {
            // El cuerpo no era JSON: se cae al error genérico de abajo
        }

        return respuestaHttp.StatusCode == System.Net.HttpStatusCode.BadRequest
            ? CrearError<T>(400, "Los datos enviados no son válidos.")
            : CrearError<T>(500, "Ocurrió un error inesperado en el servicio de votación.");
    }

    private static Respuesta<T> CrearError<T>(int codigo, string mensaje)
    {
        return new Respuesta<T>
        {
            EsCorrecto = false,
            Mensaje = mensaje,
            Codigo = codigo
        };
    }
}
