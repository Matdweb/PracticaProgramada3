using AutoMapper;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Interfaces;
using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Services;

public class VotanteServicio : IVotanteServicio
{
    private readonly IVotanteRepository _votanteRepository;
    private readonly IMapper _mapper;

    public VotanteServicio(IVotanteRepository votanteRepository, IMapper mapper)
    {
        _votanteRepository = votanteRepository;
        _mapper = mapper;
    }

    public async Task<List<VotanteDto>> ObtenerVotantesAsync(CancellationToken cancellationToken = default)
    {
        var votantes = await _votanteRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<VotanteDto>>(votantes);
    }

    public async Task<VotanteDto?> ObtenerVotantePorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var votante = await _votanteRepository.GetByIdAsync(id, cancellationToken);
        return votante is null ? null : _mapper.Map<VotanteDto>(votante);
    }

    public async Task<Respuesta<VotanteDto>> CrearVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<VotanteDto>();
        votante.Cedula = NormalizarCedula(votante.Cedula);
        votante.NombreCompleto = votante.NombreCompleto?.Trim() ?? string.Empty;

        var votanteEntity = _mapper.Map<Votante>(votante);
        if (!votanteEntity.ValidarReglaCedula())
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "La cédula debe tener 9 dígitos.";
            respuesta.Codigo = 400;
            return respuesta;
        }

        if (string.IsNullOrWhiteSpace(votanteEntity.NombreCompleto))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El nombre completo es obligatorio.";
            respuesta.Codigo = 400;
            return respuesta;
        }

        var existente = await _votanteRepository.GetVotanteByCedulaAsync(votanteEntity.Cedula, cancellationToken);
        if (existente.Id != 0)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "Ya existe un votante con esa cédula.";
            respuesta.Codigo = 409;
            return respuesta;
        }

        if (!await _votanteRepository.CreateAsync(votanteEntity, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo crear el votante.";
            respuesta.Codigo = 500;
            return respuesta;
        }

        respuesta.Dato = _mapper.Map<VotanteDto>(votanteEntity);
        return respuesta;
    }

    public async Task<Respuesta<VotanteDto>> ActualizarVotanteAsync(VotanteDto votante, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<VotanteDto>();
        votante.Cedula = NormalizarCedula(votante.Cedula);
        votante.NombreCompleto = votante.NombreCompleto?.Trim() ?? string.Empty;

        var votanteEntity = _mapper.Map<Votante>(votante);
        if (!votanteEntity.ValidarReglaCedula())
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "La cédula debe tener 9 dígitos.";
            respuesta.Codigo = 400;
            return respuesta;
        }

        if (string.IsNullOrWhiteSpace(votanteEntity.NombreCompleto))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El nombre completo es obligatorio.";
            respuesta.Codigo = 400;
            return respuesta;
        }

        var existente = await _votanteRepository.GetByIdAsync(votante.Id, cancellationToken);
        if (existente is null)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El votante no existe.";
            respuesta.Codigo = 404;
            return respuesta;
        }

        var conCedula = await _votanteRepository.GetVotanteByCedulaAsync(votanteEntity.Cedula, cancellationToken);
        if (conCedula.Id != 0 && conCedula.Id != votante.Id)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "Ya existe un votante con esa cédula.";
            respuesta.Codigo = 409;
            return respuesta;
        }

        if (!await _votanteRepository.UpdateAsync(votanteEntity, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo actualizar el votante.";
            respuesta.Codigo = 500;
            return respuesta;
        }

        respuesta.Dato = _mapper.Map<VotanteDto>(votanteEntity);
        return respuesta;
    }

    public async Task<Respuesta<VotanteDto>> EliminarVotanteAsync(int id, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<VotanteDto>();
        var existente = await _votanteRepository.GetByIdAsync(id, cancellationToken);
        if (existente is null)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El votante no existe.";
            respuesta.Codigo = 404;
            return respuesta;
        }

        if (!await _votanteRepository.DeleteAsync(id, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo eliminar el votante. Puede que ya haya emitido un voto.";
            respuesta.Codigo = 409;
            return respuesta;
        }

        return respuesta;
    }

    private static string NormalizarCedula(string cedula)
    {
        return new string((cedula ?? string.Empty).Where(char.IsDigit).ToArray());
    }
}
