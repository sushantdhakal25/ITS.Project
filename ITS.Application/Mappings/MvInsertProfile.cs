using AutoMapper;
using ITS.Application.DTOs;
using ITS.Domain.Entities;

namespace ITS.Application.Mappings
{
    public class MvInsertProfile : Profile
    {
        public MvInsertProfile()
        {
            CreateMap<MvInsert, MvInsertDto>().ReverseMap();
        }
    }

    
}
