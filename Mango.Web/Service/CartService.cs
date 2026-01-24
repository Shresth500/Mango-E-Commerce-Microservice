using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class CartService(IBaseService _baseService) : ICartService
{
    public async Task<ResponseDto?> ApplyCoupon(CartDto cart)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = SD.ApiType.POST,
            Data = cart,
            Url = SD.ShoppingCartAPI + "/api/ShoppingCartAPI/ApplyCoupon"
        });
        
    }

    public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ShoppingCartAPI + "/api/ShoppingCartAPI/GetCart/" + userId
        });
    }

    public async Task<ResponseDto?> RemoveCartAsync(int cartId)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = SD.ApiType.POST,
            Data = cartId,
            Url = SD.ShoppingCartAPI + "/api/ShoppingCartAPI/RemoveCart"
        });
    }

    public async Task<ResponseDto?> UpsertCartAsync(CartDto cart)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = SD.ApiType.POST,
            Data = cart,
            Url = SD.ShoppingCartAPI + "/api/ShoppingCartAPI/CartUpsert"
        });
    }
}
