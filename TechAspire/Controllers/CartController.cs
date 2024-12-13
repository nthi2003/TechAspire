using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly UserManager<AppUserModel> _userManager;
		public CartController(DataContext dataContext , UserManager<AppUserModel> userManager )
		{
			_dataContext = dataContext;
			_userManager =  userManager;

		}
		[HttpGet]
		public async Task<IActionResult> GetCart()
		{
			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return Unauthorized("Bạn cần đăng nhập để xem giỏ hàng.");
				}
				var cartItem = await _dataContext.CartItems
		          .Where(c => c.UserId == user.Id)
		          .Select(c => new
				  {
					  CartItemId = c.CartItemId,
					  ProductId = c.ProductId,
					  ProductName = c.ProductName,
					  Quantity = c.Quantity,
					  Price = c.Price,
					  Total = c.Quantity * c.Price,
					  Images = c.Images
				  })

		         .ToListAsync();
				if (cartItem == null || cartItem.Count == 0)
				{
					return NotFound(new { Success = false, Message = "Giỏ hàng của bạn đang trống." });
				}
				return Ok(new
				{
					Success = true,
					CartItems = cartItem
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		
		}

		[HttpPost("{productId}")]
		public async Task<IActionResult> AddToCart(int productId)
		{
			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return Unauthorized("Bạn cần đăng nhập để thực hiện hành động này.");
				}
				var product = await _dataContext.Products.FindAsync(productId);
				if (product == null)
				{
					return BadRequest(new
					{
						Success = false,
						Message = "Sản phẩm không tồn tại."
					});
				}
				var existingCartItem = await _dataContext.CartItems
					.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == user.Id);
				if(existingCartItem == null)
				{
					var newCartItem = new CartItemModel
					{
						ProductId = product.Id,
						ProductName = product.Name,
						Price = product.Price,
						Quantity = 1,
						Images = product.Images,
						UserId = user.Id
					};
					_dataContext.CartItems.Add(newCartItem);
					await _dataContext.SaveChangesAsync();

					return Ok(new
					{
						Success = true,
						Message = "Sản phẩm đã được thêm vào giỏ hàng."
					});
				}
				existingCartItem.Quantity++;
				_dataContext.CartItems.Update(existingCartItem);
				await _dataContext.SaveChangesAsync();

				return Ok(new { Success = true, Message = "Số lượng sản phẩm đã được cập nhật." });

			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Lỗi server: {ex.Message}");
			}
		}
	}
}
