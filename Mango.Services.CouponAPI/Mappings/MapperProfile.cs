using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Mappings;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Coupon,CouponDto>().ReverseMap();
    }
}
