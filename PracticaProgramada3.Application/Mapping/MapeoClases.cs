using AutoMapper;
using PracticaProgramada3.Application.Dtos;
using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Application.Mapping;

/// <summary>Perfil de AutoMapper: define cómo se convierte cada entidad en su DTO y viceversa.</summary>
public class MapeoClases : Profile
{
    public MapeoClases()
    {
        CreateMap<Votante, VotanteDto>().ReverseMap();
        CreateMap<Partido, PartidoDto>().ReverseMap();
    }
}
