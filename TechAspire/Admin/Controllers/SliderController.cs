using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechAspire.Models;

namespace TechAspire.Admin.Controllers
{
	[Route("api/Admin/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin,Sale")]

	public class SliderController : Controller
    {
        private readonly DataContext _dataContext;
		private readonly CloudinaryService _cloudinaryService;
		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public SliderController(DataContext context , CloudinaryService cloudinaryService , UserManager<AppUserModel> userManager , RoleManager<IdentityRole> roleManager )
        {

            _dataContext = context;
			_cloudinaryService = cloudinaryService;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		[HttpGet]

		public async Task<IActionResult> GetSliders()
		{
			try
			{

				var sliders = await _dataContext.Sliders
					.Include(s => s.User)
					.ToListAsync();

				
				var slidersWithRole = new List<object>();

				foreach (var slider in sliders)
				{
					var roles = await _userManager.GetRolesAsync(slider.User);
					slidersWithRole.Add(new
					{
						slider.Id,
						slider.Name,
						slider.Description,
						slider.Status,
						slider.Images,
						slider.CreatedDate,
						UserName = slider.User.UserName,
						RoleName = roles.FirstOrDefault()
					});
				}

				return Ok(new
				{
					Success = true,
					Data = slidersWithRole
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
			}
		}


		[HttpPost]
		public async Task<IActionResult> AddSlider(SliderModel slider)
		{
			try
			{

				if (slider.ImageUploads != null && slider.ImageUploads.Count > 0)
				{
					foreach (var file in slider.ImageUploads)
					{
						var imageUrl = await _cloudinaryService.UploadImageAsync(file);
						slider.Images.Add(imageUrl);
					}
				}

				

				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return Unauthorized(new { message = "Người dùng không xác thực" });
				}


				var newSlider = new SliderModel
				{
					Name = slider.Name,
					Description = slider.Description,
					Status = slider.Status,
					Images = slider.Images,
					UserId = user.Id,
					UserName = user.UserName,
					CreatedDate = DateTime.Now
				};


				_dataContext.Sliders.Add(newSlider);
				await _dataContext.SaveChangesAsync();

				return Ok(new
				{
					Success = true,
					Message = "Tạo quảng cáo thành công!",
					Data = newSlider
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateSlider(SliderModel slider, int id)
		{
			try
			{
				var existedSlider = _dataContext.Sliders.Find(id);
				if (existedSlider == null)
				{
					return BadRequest("Quản cáo không tồn tại");

				}
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return Unauthorized(new { message = "Người dùng không xác thực" });
				}

				existedSlider.Name = slider.Name;
				existedSlider.Description = slider.Description;
				existedSlider.Status = slider.Status;
				existedSlider.CreatedDate = DateTime.Now;
				existedSlider.Images = slider.Images;
				existedSlider.UserId = user.Id;
				existedSlider.UserName = user.UserName;
				existedSlider.Images.Clear();
				foreach (var file in slider.ImageUploads)
				{
					var imageUrl = await _cloudinaryService.UploadImageAsync(file);
					existedSlider.Images.Add(imageUrl);
				}

				_dataContext.Sliders.Update(existedSlider);
				await _dataContext.SaveChangesAsync();
				return Ok(new
				{
					Success = true,
					Message = "Cập nhật quảng cáo thành công",
					Data = existedSlider
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
			}

		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSlider(int id)
		{
			try
			{
				var slider = await _dataContext.Sliders.FindAsync(id);
				if (slider == null)
				{
					return NotFound(new { message = "Quảng cáo không tồn tại" });
				}


				foreach (var imageUrl in slider.Images)
				{
					await DeleteImageFromCloudinary(imageUrl);
				}

				_dataContext.Sliders.Remove(slider);
				await _dataContext.SaveChangesAsync();

				return Ok(new { Success = true, message = "Quảng cáo đã được xóa thành công" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi khi xóa : {ex.Message}" });
			}
		}
		[HttpDelete("DeleteImage")]
		public async Task<IActionResult> DeleteImage([FromQuery] string imageUrl)
		{
			try
			{
				if (string.IsNullOrEmpty(imageUrl))
				{
					return BadRequest(new { message = "URL ảnh không hợp lệ" });
				}

				await DeleteImageFromCloudinary(imageUrl);
				return Ok(new { Success = true, message = "Ảnh đã được xóa thành công" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi khi xóa ảnh: {ex.Message}" });
			}
		}
		private async Task DeleteImageFromCloudinary(string imageUrl)
		{
			try
			{

				var publicId = imageUrl.Split('/').Last().Split('.').First();


				var result = await _cloudinaryService.DestroyAsync(publicId);

				if (result.Result != "ok")
				{
					throw new Exception($"Không thể xóa ảnh: {imageUrl}");
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Lỗi khi xóa ảnh: {ex.Message}");
			}
		}





	}
}
