using AutoMapper;
using model;
using modelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Master, NacionLicitanteDto>();
            CreateMap<BiddingParticipant, BiddingParticipantDTO>();
            CreateMap<Master, BlogDto>();
            CreateMap<PQRSD, PQRSDDto>();
        }
    }
}
