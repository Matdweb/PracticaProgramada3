using AutoMapper;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Application.Interfaces;

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
}
