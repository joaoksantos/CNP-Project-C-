using AutoMapper;
using PROJETOCNP.DTOs;
using PROJETOCNP.Models;

namespace PROJETOCNP.Mappings
{
    public class CriminosoProfile : Profile
    {
        public CriminosoProfile()
        {
            CreateMap<Criminoso, CriminosoGetDto>();
            CreateMap<Criminoso, CriminosoUpdateDto>();
        }
    }
}