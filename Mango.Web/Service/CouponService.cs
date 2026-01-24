using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class CouponService(IBaseService _baseService) : ICouponService
{
    public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = couponDto,
            Url = SD.CouponAPIBase + "/api/CouponAPI"
        });
    }

    public async Task<ResponseDto?> DeleteCouponsAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.CouponAPIBase + "/api/CouponAPI/" + id
        });
    }

    public async Task<ResponseDto?> GetAllCouponsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CouponAPIBase + "/api/CouponAPI"
        });
    }

    public async Task<ResponseDto?> GetCouponAsync(string couponCode)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CouponAPIBase + "/api/CouponAPI/GetByCode/" + couponCode
        });
    }

    public async Task<ResponseDto?> GetCouponByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.CouponAPIBase + "/api/CouponAPI/" + id
        });
    }

    public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.PUT,
            Data = couponDto,
            Url = SD.CouponAPIBase + "/api/CouponAPI"
        });
    }
}
