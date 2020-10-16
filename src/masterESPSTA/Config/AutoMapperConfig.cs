using AutoMapper;
using model;
using modelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masterESPSTA.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Master, NacionLicitanteDto>();
            CreateMap<BiddingParticipant, BiddingParticipantDTO>();
        }
    }
}
