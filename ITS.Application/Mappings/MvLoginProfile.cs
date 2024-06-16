using AutoMapper;
using ITS.Application.DTOs;
using ITS.Domain.Entities;

namespace ITS.Application.Mappings
{
    public class MvLoginProfile : Profile
    {
        public MvLoginProfile()
        {
            CreateMap<MvLogin, MvLoginDto>().ReverseMap();
        }
    }

    
}
