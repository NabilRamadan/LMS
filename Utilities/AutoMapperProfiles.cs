using AutoMapper;
using CRUDApi.DTOs;
using CRUDApi.Entities;

namespace CRUDApi.Utilities
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DisplayName));
        }

    }
}
