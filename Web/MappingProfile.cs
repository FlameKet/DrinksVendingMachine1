using AutoMapper;
using Web.Application;
using Web.Models;

namespace Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<MachineStack<Coin>, CoinModel>()
                .ForMember(dest => dest.Par, opt => opt.MapFrom(src => src.Entity.Par))
                .ForMember(dest => dest.Blocking, opt => opt.MapFrom(src => src.Entity.Blocking)); 
            CreateMap<MachineStack<Drink>, DrinkModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Entity.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Entity.Image))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Entity.Volume))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Entity.Price));
        }
    }
}
