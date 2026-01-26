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
}
