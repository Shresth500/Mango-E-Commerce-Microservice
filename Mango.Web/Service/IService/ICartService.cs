using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface ICartService
{
    Task<ResponseDto?> GetCartByUserIdAsync(string userId);
    Task<ResponseDto?> UpsertCartAsync(CartDto cart);
    Task<ResponseDto?> RemoveCartAsync(int cartId);
    Task<ResponseDto?> ApplyCoupon(CartDto cart);
    Task<ResponseDto?> EmailCart(CartDto cartDto);
}
