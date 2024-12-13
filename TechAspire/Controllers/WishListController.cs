using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechAspire.Models;

namespace TechAspire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class WishListController : Controller
    {
        private readonly DataContext _dataContext;
		private readonly UserManager<AppUserModel> _userManager;

		public WishListController(DataContext context, UserManager<AppUserModel> userManager  )
        {
            _dataContext = context;
			_userManager = userManager;

		}
        [HttpGet]

        public async Task<IActionResult> GetWishList()
        {
            try
            {
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return Unauthorized("Bạn cần đăng nhập để thực hiện chức năng này.");
				}
				var wishlist = await _dataContext.WishLists
				.Include(w => w.Product)
				.Where(w => w.Email == user.Email)
				.ToListAsync();

				if (wishlist == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Mục yêu thích chưa tồn tại"
                    });
                }
                return Ok(new
                {
                    Success = false,
                    Data = wishlist
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server : {ex.Message} ");
            }

        }
		[HttpPost("{productId}")]
		public async Task<IActionResult> AddWishList(int productId)
        {

            try
            {
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return Unauthorized("Bạn cần đăng nhập để thực hiện chức năng này.");
				}
			
				var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
				if (product == null)
                {

                    return BadRequest(new
                    {
                        Success = false,
                        Message = " Không tìm thấy sản phẩm"
                    });
                   
                }
                var existingWishList = await _dataContext.WishLists.FirstOrDefaultAsync(w => w.Id == productId && w.Email == user.Email);
                if (existingWishList != null)
                {
					return BadRequest(new
					{
						Success = false,
						Message = "Sản phẩm đã có trong danh sách yêu thích ."
					});
				}
                var newWishList = new WishListModel
                {
                    ProductId = productId,
                    Email = user.Email
                };
				_dataContext.WishLists.Add(newWishList);
				await _dataContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Đã thêm sản phẩm vào danh sách yêu thích ."
                });

			}
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server : {ex.Message} ");
            }

        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteWishList(int Id)
        {
            try
            {
				var wishlist = await _dataContext.WishLists.FindAsync(Id);
				if (wishlist == null)
				{
					return BadRequest(new
					{
						Success = false,
						Message = "Không tìm thấy sản phẩm yêu thích"
					});
				}
               _dataContext.WishLists.Remove(wishlist);
                await _dataContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Sản phẩm yêu thích đã được xóa"
                });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server : {ex.Message} ");
			}

		}

    }
}
