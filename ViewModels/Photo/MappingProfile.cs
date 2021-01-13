using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.ViewModels.Photo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Photo, IndexViewModel>();
            CreateMap<Models.Photo, DetailViewModel>();
            CreateMap<Models.PhotoLocation, PhotoLocationViewModel>().ForMember(p=>p.Source,m=>m.MapFrom(s=>s.Photo.Source))
                                                                     .ForMember(p => p.Url, m => m.MapFrom(s => s.Photo.Url))
                                                                     .ForMember(p => p.LocationName, m => m.MapFrom(s => s.Location.Name));
        }
    }
}
