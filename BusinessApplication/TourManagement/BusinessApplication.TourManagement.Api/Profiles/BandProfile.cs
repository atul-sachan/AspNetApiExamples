using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Profiles
{
    public class BandProfile: Profile
    {
        public BandProfile()
        {
            CreateMap<Entities.Band, Dtos.Band>();
        }
    }
}
