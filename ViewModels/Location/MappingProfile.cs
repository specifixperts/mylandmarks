using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Location
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Location, CityViewModel>().ReverseMap().ForMember(c=>c.Name,s=>s.MapFrom(d=> $"{d.Name}, {d.Country}"));
            CreateMap<Models.Location, IndexViewModel>().ReverseMap();
            CreateMap<Models.Location, CountryViewModel>().ReverseMap().ForMember(l=>l.Latitude,s=>s.MapFrom(c=>c.Latlng[0]))
                                                                       .ForMember(l => l.Longitude, s => s.MapFrom(c => c.Latlng[1]));
        }
    }
}
