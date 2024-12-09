using System.ComponentModel.DataAnnotations;
using TechAspire.Models;

public class OrderModel
{
    [Key]
    public string OrderCode { get; set; }  // Khóa chính là OrderCode

    public string UserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; }

    public string UserId { get; set; }
    public AppUserModel User { get; set; }

    public int ShippingId { get; set; }
    public ShippingModel Shipping { get; set; }

    public int? CouponId { get; set; }
    public CouponModel Coupon { get; set; }
}
