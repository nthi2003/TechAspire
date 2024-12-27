using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin.Controllers
{
	[Route("api/Admin/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class ShippingController : Controller
	{
		private readonly DataContext _dataContext;
		public ShippingController(DataContext context)
		{

			_dataContext = context;

		}
		[HttpGet]
		public async Task<IActionResult> GetShipping()
		{
			try
			{
				var shipping = await _dataContext.Shippings.ToListAsync();
				if (shipping == null) {

					return NotFound(new { message = "Phí vận chuyển không tồn tại" });
				}
				return Ok(new
				{
					Success = true,
					Data = shipping
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}
		[HttpPost]
		public async Task<IActionResult> CreateShipping(ShippingModel shipping)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(shipping.City) ||
			    string.IsNullOrWhiteSpace(shipping.District) ||
			    string.IsNullOrWhiteSpace(shipping.Ward) ||
			    shipping.Price <= 0)
				{
					return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ!" });
				}

				var existingShipping = await _dataContext.Shippings
					.AnyAsync(x => x.City == shipping.City &&
								   x.District == shipping.District &&
								   x.Ward == shipping.Ward

					);
				if (existingShipping)
				{
					return BadRequest(new { Success = false, message = "Dữ liệu bị trùng lặp!" });
				}

				var newShipping = new ShippingModel
				{
					City = shipping.City,
					District = shipping.District,
					Ward = shipping.Ward,
					Price = shipping.Price,
					
				};
				_dataContext.Shippings.Add(shipping);
				await _dataContext.SaveChangesAsync();

				return Ok(new { success = true, message = "Thêm vận chuyển thành công!" , Data = newShipping
				});

			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateShipping(ShippingModel shipping , int id)
		{
			try
			{
				var existingShipping = await _dataContext.Shippings.FindAsync(id);
				if (existingShipping == null)
				{
					return NotFound(new { message = "Phí vận chuyển không tồn tại" });
				}
				existingShipping.City = shipping.City;
				existingShipping.Ward = shipping.Ward;
				existingShipping.District = shipping.District;
				existingShipping.Price = shipping.Price;


				_dataContext.Shippings.Update(existingShipping);
				await _dataContext.SaveChangesAsync();

				return Ok(new
				{
					Success = true,
					Message = "Cập nhật phí vận chuyển thành công",
					Data = existingShipping
				});


			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteShiping(int id)
		{
			try
			{
				var existingShipping = await _dataContext.Shippings.FindAsync(id);
				if (existingShipping == null)
				{
					return NotFound(new { message = "Phí vận chuyển không tồn tại" });
				}
				_dataContext.Shippings.Remove(existingShipping);
				await _dataContext.SaveChangesAsync();
				return Ok(new
				{
					Success = true,
					Message = "Xóa phí vận chuyển thành công",
					
				});


			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}

	}
}
