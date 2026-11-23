using AutoMapper;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Services;

public class PartidoServicio : IPartidoServicio
{
    // Inversión de dependencias (SOLID): se depende de la interfaz, no de la clase concreta
    private readonly IPartidoRepository _partidoRepository;
    private readonly IMapper _mapper;

    public PartidoServicio(IPartidoRepository partidoRepository, IMapper mapper)
    {
        _partidoRepository = partidoRepository;
        _mapper = mapper;
    }

    public async Task<List<PartidoDto>> ObtenerPartidosAsync(CancellationToken cancellationToken = default)
    {
        var partidos = await _partidoRepository.GetAllPartidosAsync(cancellationToken);
        return _mapper.Map<List<PartidoDto>>(partidos);
    }

    public async Task<PartidoDto?> ObtenerPartidoPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var partido = await _partidoRepository.GetPartidoByIdAsync(id, cancellationToken);
        return partido.Id == 0 ? null : _mapper.Map<PartidoDto>(partido);
    }

    public async Task<Respuesta<PartidoDto>> CrearPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<PartidoDto>();
        partido.Nombre = NormalizarNombre(partido.Nombre);

        var partidoEntity = _mapper.Map<Partido>(partido);
        if (!partidoEntity.ValidarReglaNombre())
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El nombre del partido debe tener entre 2 y 100 caracteres.";
            respuesta.Codigo = 400;
            return respuesta;
        }

        var existente = await _partidoRepository.GetPartidoByNombreAsync(partidoEntity.Nombre, cancellationToken);
        if (existente.Id != 0)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "Ya existe un partido con ese nombre.";
            respuesta.Codigo = 409;
            return respuesta;
        }

        if (!await _partidoRepository.CreateAsync(partidoEntity, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo crear el partido.";
            respuesta.Codigo = 500;
            return respuesta;
        }

        respuesta.Mensaje = "Partido creado correctamente.";
        respuesta.Dato = _mapper.Map<PartidoDto>(partidoEntity);
        return respuesta;
    }

    public async Task<Respuesta<PartidoDto>> ActualizarPartidoAsync(PartidoDto partido, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<PartidoDto>();
        partido.Nombre = NormalizarNombre(partido.Nombre);

        var partidoEntity = _mapper.Map<Partido>(partido);
        if (!partidoEntity.ValidarReglaNombre())
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El nombre del partido debe tener entre 2 y 100 caracteres.";
            respuesta.Codigo = 400;
            return respuesta;
        }

        var existente = await _partidoRepository.GetPartidoByIdAsync(partido.Id, cancellationToken);
        if (existente.Id == 0)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El partido no existe.";
            respuesta.Codigo = 404;
            return respuesta;
        }

        var conNombre = await _partidoRepository.GetPartidoByNombreAsync(partidoEntity.Nombre, cancellationToken);
        if (conNombre.Id != 0 && conNombre.Id != partido.Id)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "Ya existe un partido con ese nombre.";
            respuesta.Codigo = 409;
            return respuesta;
        }

        if (!await _partidoRepository.UpdateAsync(partidoEntity, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo actualizar el partido.";
            respuesta.Codigo = 500;
            return respuesta;
        }

        respuesta.Mensaje = "Partido actualizado correctamente.";
        respuesta.Dato = _mapper.Map<PartidoDto>(partidoEntity);
        return respuesta;
    }

    public async Task<Respuesta<PartidoDto>> EliminarPartidoAsync(int id, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<PartidoDto>();
        var existente = await _partidoRepository.GetPartidoByIdAsync(id, cancellationToken);
        if (existente.Id == 0)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El partido no existe.";
            respuesta.Codigo = 404;
            return respuesta;
        }

        if (!await _partidoRepository.DeleteAsync(id, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo eliminar el partido. Puede que ya tenga votos registrados.";
            respuesta.Codigo = 409;
            return respuesta;
        }

        respuesta.Mensaje = "Partido eliminado correctamente.";
        return respuesta;
    }

    /// <summary>Quita espacios al inicio, al final y los espacios repetidos: "  Partido   Azul " pasa a "Partido Azul".</summary>
    private static string NormalizarNombre(string nombre)
    {
        return string.Join(' ', (nombre ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
