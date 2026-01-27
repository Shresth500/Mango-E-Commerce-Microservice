using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers;

public class CartController(ICartService _cartService, IOrderService _orderService) : Controller
{
    ResponseDto _responseDto = new ResponseDto { };

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadCartBasedOnLoggedInUser());
    }

    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        ResponseDto? response = await _cartService.RemoveCartAsync(cartDetailsId);
        if (response != null & response!.IsSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
    {

        ResponseDto? response = await _cartService.ApplyCoupon(cartDto);
        if (response != null & response!.IsSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EmailCart(CartDto cartDto)
    {
        CartDto cart = await LoadCartBasedOnLoggedInUser();
        cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
        ResponseDto? response = await _cartService.EmailCart(cart);
        if (response != null & response!.IsSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
    {
        cartDto.CartHeader.CouponCode = "";
        ResponseDto? response = await _cartService.ApplyCoupon(cartDto);
        if (response != null & response!.IsSuccess)
        {
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    public async Task<CartDto> LoadCartBasedOnLoggedInUser()
    {
        var userid = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()!.Value;
        var response = await _cartService.GetCartByUserIdAsync(userid!);
        if (response != null && response.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)!)!;
        }
        return new CartDto();
    }

    [Authorize]
    public async Task<IActionResult> CheckOut()
    {
        return View(await LoadCartBasedOnLoggedInUser());
    }
    [Authorize]
    [HttpPost]
    [ActionName("Checkout")]
    public async Task<IActionResult> CheckOut(CartDto cartDto)
    {
        CartDto cart = await LoadCartBasedOnLoggedInUser();
        cart.CartHeader.Phone = cartDto.CartHeader.Phone;
        cart.CartHeader.Email = cartDto.CartHeader.Email;
        cart.CartHeader.Name = cartDto.CartHeader.Name;

        var response = await _orderService.CreateOrder(cart);
        OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response!.Result)!)!;

        if (response != null && response.IsSuccess)
        {
            var domain = Request.Scheme + "://" + Request.Host;
            var stripRequestDto = new StripeRequestDto
            {
                CancelUrl = domain + "/cart/checkout",
                ApprovedUrl = domain + "/cart/Confirmation?orderid=" + orderHeaderDto.OrderHeaderId,
                OrderHeader = orderHeaderDto,
            };
            response = await _orderService.CreateStripeSession(stripRequestDto);
            var stripeResponse = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(response!.Result)!);
            Response!.Headers.Add("Location",stripeResponse.StripeSessionUrl!);
            return new StatusCodeResult(303);
        }
        return View(cart);
    }

    [Authorize]
    public async Task<IActionResult> Confirmation(int orderid)
    {
        return View(orderid);
    }
}
