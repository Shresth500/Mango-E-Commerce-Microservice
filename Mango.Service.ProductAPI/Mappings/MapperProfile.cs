using AutoMapper;
using Mango.Service.ProductAPI.Models.Dto;
using Mango.Service.ProductAPI.Models;

namespace Mango.Services.ProductAPI.Mappings;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
    }
}
