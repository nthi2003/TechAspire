using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin.Controllers
{
	[Route("api/Admin/[controller]")]
	[ApiController]
	//[Authorize(Roles = "Admin")]
	public class CouponController : Controller
	{
		private readonly DataContext _dataContext;


		public CouponController(DataContext context)
		{

			_dataContext = context;
		}
		[HttpGet]
		public async Task<IActionResult> GetCoupon()
		{
			try
			{
				var coupon = await _dataContext.Coupons.ToListAsync();
				if (coupon == null) {
					return StatusCode(400, "Không tìm thấy mã giảm giá" );
				}
				return Ok(new
				{
					Success = true,
					Data = coupon
				});
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}
		[HttpPost]
		public async Task<IActionResult> CreateCoupon (CouponModel coupon)
		{
			try
			{
				var coupons = await _dataContext.Coupons.ToListAsync();

				if (coupons == null) {
					return StatusCode(400, "Không tìm thấy mã giảm giá");
				}
				var newCoupon = new CouponModel
				{
					Name = coupon.Name,
					Description = coupon.Description,
					DateStart = DateTime.Now,
					DateExpired = coupon.DateExpired,
					Quantity = coupon.Quantity,
					Status = coupon.Status,
					DiscountPercentage = coupon.DiscountPercentage

				};
				_dataContext.Coupons.Add(newCoupon);
				await _dataContext.SaveChangesAsync();
				return Ok(new
				{
					Message = "Đã thêm mã giảm giá thành công",
					Data = newCoupon
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCoupon (CouponModel coupon, int id)
		{
			try
			{
				var existingCoupons = await _dataContext.Coupons.FindAsync(id);
				if (existingCoupons == null)
				{
					return NotFound(new { message = "Mã giảm giá không tồn tại" });
				}
				existingCoupons.Name = coupon.Name;
				existingCoupons.Description = coupon.Description;
				existingCoupons.DateStart = coupon.DateStart;
				existingCoupons.DateExpired = coupon.DateExpired;
				existingCoupons.DiscountPercentage = coupon.DiscountPercentage;
				existingCoupons.Status = existingCoupons.Status;
				_dataContext.Coupons.Update(existingCoupons);
				await _dataContext.SaveChangesAsync();
				return Ok(new
				{
					Message = "Cập nhật thành công",
					Data = existingCoupons
				});



			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCounpon (int id)
		{
			try
			{

				var coupon = await _dataContext.Coupons.FindAsync(id);
				if (coupon == null)
				{
					return NotFound(new { message = "Mã giảm giá không tồn tại" });
				}
				_dataContext.Coupons.Remove(coupon);
				await _dataContext.SaveChangesAsync();
				return Ok(new
				{
					Message = "Đã xóa mã giảm giá"
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
			}
		}
	}
}
