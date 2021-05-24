using System.Linq;
using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;

namespace DatingApp.Api.Profiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl , opt=> opt.MapFrom(src=>src.Photos.FirstOrDefault(x=> x.IsMain).Url))
                .ForMember(dest => dest.Age, opt=> opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>().ReverseMap();
        }
    }
}