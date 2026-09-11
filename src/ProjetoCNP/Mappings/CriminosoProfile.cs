using AutoMapper;
using PROJETOCNP.DTOs;
using PROJETOCNP.Models;

namespace PROJETOCNP.Mappings
{
    public class CriminosoProfile : Profile
    {
        public CriminosoProfile()
        {
            CreateMap<CriminosoCreateDto, Criminoso>();
            CreateMap<CriminosoUpdateDto, Criminoso>();
            CreateMap<Criminoso, CriminosoGetDto>();
            CreateMap<Criminoso, CriminosoUpdateDto>();
        }
    }
}