using AutoMapper;
using Mango.Service.ShoppingCartAPI.Models;
using Mango.Service.ShoppingCartAPI.Models.Dto;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Mango.Services.ShoppingCartAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ShoppingCartAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartAPIController(AppDbContext _dbContext, IMapper _mapper, IProductService _productService,ICouponService _couponService) : ControllerBase
{
    private ResponseDto _response = new ResponseDto();

    [HttpGet("GetCart/{userId}")]
    public async Task<IActionResult> GetCart([FromRoute] string userId)
    {
        try
        {
            var cart = new CartDto
            {
                CartHeader = _mapper.Map<CartHeaderDto>(_dbContext.CartHeaders.First(x => x.UserId == userId))
            };
            cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_dbContext.CartDetails.Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId));
            _response.IsSuccess = true;

            var productDtos = await _productService.GetProducts();
            foreach (var item in cart.CartDetails)
            {
                item.Product = productDtos.FirstOrDefault(x => x.ProductId == item.ProductId);
                cart.CartHeader.CartTotal += (item.Count * item.Product!.Price);
            }
            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                {
                    cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                    cart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }
            _response.Result = cart;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("ApplyCoupon")]
    public async Task<IActionResult> ApplyCoupon([FromBody]CartDto cart)
    {
        try
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cart.CartHeader.CartHeaderId);
            cartFromDb!.CouponCode = cart.CartHeader!.CouponCode;
            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("CartUpsert")]
    public async Task<IActionResult> CartUpsert(CartDto cartDto)
    {
        try
        {
            var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
            if (cartHeaderFromDb == null)
            {
                //create header and details
                CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                _dbContext.CartHeaders.Add(cartHeader);
                await _dbContext.SaveChangesAsync();
                cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                //if header is not null
                //check if details has same product
                var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                if (cartDetailsFromDb == null)
                {
                    //create cartdetails
                    cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    //update count in cart details
                    cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                    cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    _dbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _dbContext.SaveChangesAsync();
                }
            }
            _response.Result = cartDto;
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message.ToString();
            _response.IsSuccess = false;
        }
        return Ok(_response);
    }

    [HttpPost("RemoveCart")]
    public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
    {
        try
        {
            CartDetails cartDetails = _dbContext.CartDetails
               .First(u => u.CartDetailsId == cartDetailsId);

            int totalCountofCartItem = _dbContext.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
            _dbContext.CartDetails.Remove(cartDetails);
            if (totalCountofCartItem == 1)
            {
                var cartHeaderToRemove = await _dbContext.CartHeaders
                   .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                _dbContext.CartHeaders.Remove(cartHeaderToRemove);
            }
            await _dbContext.SaveChangesAsync();

            _response.Result = true;
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message.ToString();
            _response.IsSuccess = false;
        }
        return _response;
    }

}
