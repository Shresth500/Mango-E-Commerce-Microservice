using AutoMapper;
using Mango.Service.ShoppingCartAPI.Models.Dto;
using Mango.Service.ShoppingCartAPI.Models;

namespace Mango.Services.ProductAPI.Mappings;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
        CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
    }
}
