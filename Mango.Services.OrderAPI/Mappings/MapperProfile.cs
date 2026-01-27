using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI.Mappings;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

        CreateMap<CartDetailsDto, OrderDetailsDto>()
        .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
        .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price));

        CreateMap<OrderDetailsDto, CartDetailsDto>();

        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
        CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();

    }
}
