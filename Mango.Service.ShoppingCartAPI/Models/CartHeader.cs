using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Service.ShoppingCartAPI.Models;

public class CartHeader
{
    [Key]
    public int CartHeaderId { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    // public List<CartDetails> CartDetails { get; set; } = new List<CartDetails>();

    [NotMapped]
    public double Discount { get; set; }
    [NotMapped]
    public double CartTotal { get; set; }
}
