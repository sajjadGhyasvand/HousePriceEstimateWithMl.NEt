using AutoMapper;
using MLHousePrice.Models.Entities;
using MLHousePrice.Models.Services.DTOs;

namespace MLHousePrice.Models.Services.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Advertisement, AdvertisementListDto>()
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name));

            CreateMap<CreateAdvertisementDto, Advertisement>();

            CreateMap<Advertisement, AdvertisementDetailsDto>();

            CreateMap<PredictPriceInputDto, Advertisement>();
        }
    }
}
