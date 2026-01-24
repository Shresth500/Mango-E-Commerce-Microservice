using Mango.Service.ShoppingCartAPI.Models.Dto;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;

namespace Mango.Service.ShoppingCartAPI.Service;

public class CouponService(IHttpClientFactory httpClientFactory) : ICouponService
{
    public async Task<CouponDto> GetCoupon(string couponCode)
    {
        var client = httpClientFactory.CreateClient("Coupon");
        var response = await client.GetAsync($"/api/CouponAPI/GetByCode/{couponCode}");
        var apiContet = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
        if (resp != null && resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result)!)!;
        }
        return new CouponDto();
    }
}
