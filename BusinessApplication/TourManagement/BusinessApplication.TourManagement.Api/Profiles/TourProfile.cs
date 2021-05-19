using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Profiles
{
    public class TourProfile : Profile
    {
        public TourProfile()
        {
            CreateMap<Entities.Tour, Dtos.Tour>()
                .ForMember(d => d.Band, o => o.MapFrom(s => s.Band.Name));

            CreateMap<Entities.Tour, Dtos.TourWithEstimatedProfit>()
                .ForMember(d => d.Band, o => o.MapFrom(s => s.Band.Name));

            CreateMap<Dtos.TourForCreation, Entities.Tour>();
            CreateMap<Dtos.TourWithManagerForCreation, Entities.Tour>();
        }
    }
}
