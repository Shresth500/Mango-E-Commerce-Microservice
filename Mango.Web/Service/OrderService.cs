using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class OrderService(IBaseService _baseService) : IOrderService
{
    public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            Url = SD.OrderAPI + "/api/OrderAPI/CreateOrder",
            Data = cartDto,
            ApiType = SD.ApiType.POST,

        });
    }

    public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            Url = SD.OrderAPI + "/api/OrderAPI/CreateStripeSession",
            Data = stripeRequestDto,
            ApiType = SD.ApiType.POST,

        });
    }

    public async Task<ResponseDto?> GetAllOrder(string? userId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.OrderAPI + "/api/OrderAPI/GetOrders?userId=" + userId
        });
    }

    public async Task<ResponseDto?> GetOrder(int orderId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.OrderAPI + "/api/OrderAPI/GetOrder/" + orderId
        });
    }

    public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = newStatus,
            Url = SD.OrderAPI + "/api/OrderAPI/UpdateOrderStatus/" + orderId
        });
    }

    public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
    {
        return await _baseService.SendAsync( new RequestDto
        {
            Url = SD.OrderAPI + "/api/OrderAPI/ValidateStripeSession",
            Data = orderHeaderId,
            ApiType = SD.ApiType.POST
        });
    }
}
