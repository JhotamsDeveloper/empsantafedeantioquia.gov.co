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
            CreateMap<Master, NacionLicitanteDto>();
            CreateMap<Master, BlogDto>();
            CreateMap<Master, BrigadeDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<BiddingParticipant, BiddingParticipantDTO>();
            CreateMap<PQRSD, PQRSDDto>();
            CreateMap<Document, DocumentDTO>();
            CreateMap<Product, ProductDto>();
        }
    }
}
