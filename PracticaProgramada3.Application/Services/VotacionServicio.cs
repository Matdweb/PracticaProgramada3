using AutoMapper;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Services;

public class VotacionServicio : IVotacionServicio
{
    private readonly IVotanteRepository _votanteRepository;
    private readonly IPartidoRepository _partidoRepository;
    private readonly IVotoRepository _votoRepository;
    private readonly IMapper _mapper;

    public VotacionServicio(
        IVotanteRepository votanteRepository,
        IPartidoRepository partidoRepository,
        IVotoRepository votoRepository,
        IMapper mapper)
    {
        _votanteRepository = votanteRepository;
        _partidoRepository = partidoRepository;
        _votoRepository = votoRepository;
        _mapper = mapper;
    }

    public async Task<Respuesta<VotanteDto>> VerificarVotanteAsync(string cedula, CancellationToken cancellationToken = default)
    {
        var habilitado = await ObtenerVotanteHabilitadoAsync(cedula, cancellationToken);
        if (!habilitado.EsCorrecto)
        {
            return CrearError<VotanteDto>(habilitado.Codigo, habilitado.Mensaje);
        }

        return new Respuesta<VotanteDto>
        {
            Mensaje = "Cédula habilitada para votar.",
            Dato = _mapper.Map<VotanteDto>(habilitado.Dato)
        };
    }

    public async Task<Respuesta<PartidoDto>> EmitirVotoAsync(EmitirVotoDto voto, CancellationToken cancellationToken = default)
    {
        // 1. La cédula debe estar inscrita y no haber votado
        var habilitado = await ObtenerVotanteHabilitadoAsync(voto.Cedula, cancellationToken);
        if (!habilitado.EsCorrecto || habilitado.Dato is null)
        {
            return CrearError<PartidoDto>(habilitado.Codigo, habilitado.Mensaje);
        }

        var votante = habilitado.Dato;

        // 2. El partido debe existir
        var partido = await _partidoRepository.GetPartidoByIdAsync(voto.PartidoId, cancellationToken);
        if (partido.Id == 0)
        {
            return CrearError<PartidoDto>(404, "El partido seleccionado no existe.");
        }

        // 3. Guardar el voto
        var nuevoVoto = new Voto
        {
            VotanteId = votante.Id,
            PartidoId = partido.Id,
            FechaVoto = DateTime.UtcNow
        };

        if (!await _votoRepository.CreateVotoAsync(nuevoVoto, cancellationToken))
        {
            // Si dos solicitudes con la misma cédula llegan a la vez, el índice único de la base de datos
            // rechaza la segunda: se revisa si fue por eso para dar el mensaje correcto.
            if (await _votoRepository.ExisteVotoDeVotanteAsync(votante.Id, cancellationToken))
            {
                return CrearError<PartidoDto>(409, "Esta cédula ya emitió su voto.");
            }

            return CrearError<PartidoDto>(500, "No se pudo registrar el voto. Intenta de nuevo.");
        }

        return new Respuesta<PartidoDto>
        {
            Mensaje = "Tu voto fue registrado correctamente.",
            Dato = _mapper.Map<PartidoDto>(partido)
        };
    }

    public async Task<List<ResultadoPartidoDto>> ObtenerResultadosAsync(CancellationToken cancellationToken = default)
    {
        return await _votoRepository.GetResultadosAsync(cancellationToken);
    }

    /// <summary>
    /// Aplica las reglas comunes para poder votar: formato de la cédula, que esté inscrita y que no haya votado.
    /// </summary>
    /// <param name="cedula">Cédula escrita por la persona (acepta guiones y espacios).</param>
    /// <returns>Respuesta con el votante en Dato si puede votar; si no, el código y mensaje del problema.</returns>
    private async Task<Respuesta<Votante>> ObtenerVotanteHabilitadoAsync(string cedula, CancellationToken cancellationToken)
    {
        var cedulaNormalizada = NormalizarCedula(cedula);

        // La regla de formato vive en la entidad
        if (!new Votante { Cedula = cedulaNormalizada }.ValidarReglaCedula())
        {
            return CrearError<Votante>(400, "La cédula debe tener 9 dígitos.");
        }

        var votante = await _votanteRepository.GetVotanteByCedulaAsync(cedulaNormalizada, cancellationToken);
        if (votante.Id == 0)
        {
            return CrearError<Votante>(404, "La cédula no está inscrita en el padrón electoral.");
        }

        var yaVoto = await _votoRepository.ExisteVotoDeVotanteAsync(votante.Id, cancellationToken);
        if (!votante.ValidarReglaPuedeVotar(yaVoto))
        {
            return CrearError<Votante>(409, "Esta cédula ya emitió su voto.");
        }

        return new Respuesta<Votante> { Dato = votante };
    }

    /// <summary>Deja solo los dígitos de la cédula (quita guiones y espacios): "1-0234-0567" pasa a "102340567".</summary>
    private static string NormalizarCedula(string cedula)
    {
        return new string((cedula ?? string.Empty).Where(char.IsDigit).ToArray());
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
