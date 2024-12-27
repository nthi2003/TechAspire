using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechAspire.Models;

namespace TechAspire.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly UserManager<AppUserModel> _userManager;
		private readonly IConfiguration _configuration;

		public CartController(DataContext dataContext, UserManager<AppUserModel> userManager, IConfiguration configuration)
		{
			_dataContext = dataContext;
			_userManager = userManager;
			_configuration = configuration;
		}
		[HttpGet]
		public async Task<IActionResult> GetCart(int cartId)
		{
			try
			{
	
				var cart = await _dataContext.Cart
					.Include(c => c.Items)
					.FirstOrDefaultAsync(c => c.Id == cartId);

			
				if (cart == null)
				{
					return NotFound("Giỏ hàng không tồn tại"); 
				}

			

				return Ok(cart); 
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi xảy ra: " + ex.Message);
				return StatusCode(500, "Lỗi hệ thống. Vui lòng thử lại sau.");
			}
		}

		[HttpPost("{productId}")]
	
		public async Task<IActionResult> AddToCart(int productId)
		{
			try
			{
				
				var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

				var product = await _dataContext.Products.FindAsync(productId);
				if (product == null)
				{
					return NotFound(new { message = "Sản phẩm không tồn tại." });
				}

			
				var cart = await _dataContext.Cart
					.Include(c => c.Items)
					.FirstOrDefaultAsync(c => c.UserId == userId);
				if (cart == null)
				{
					cart = new CartModel
					{
						UserId = userId,
						CreatedAt = DateTime.UtcNow,
						UpdatedAt = DateTime.UtcNow,
						Items = new List<CartItemModel>()
					};
					_dataContext.Cart.Add(cart);
				}

				// Thêm sản phẩm vào giỏ hàng
				var cartItem = cart.Items.FirstOrDefault(c => c.ProductId == productId);
				if (cartItem == null)
				{
					cartItem = new CartItemModel
					{
						ProductId = productId,
						Product = product,
						Quantity = 1,
						Price = product.Price
					};
					cart.Items.Add(cartItem);
				}
				else
				{
					cartItem.Quantity++;
				}

				// Lưu thay đổi
				await _dataContext.SaveChangesAsync();

				return Ok(new { message = "Thêm sản phẩm vào giỏ hàng thành công.", Data = cart });
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi xảy ra: " + ex.Message);
				return StatusCode(500, "Lỗi hệ thống. Vui lòng thử lại sau.");
			}
		}
	}
}