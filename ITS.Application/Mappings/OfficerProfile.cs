using AutoMapper;
using ITS.Application.DTOs;
using ITS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Application.Mappings
{
    public class OfficerProfile : Profile
    {
        public OfficerProfile()
        {
            CreateMap<Officer, OfficerDto>().ReverseMap();
        }
    }
}
