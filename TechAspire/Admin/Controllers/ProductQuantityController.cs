using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
	//[Authorize(Roles = "Admin")]
	public class ProductQuantityController : Controller
    {

        private readonly DataContext _dataContext;
        private readonly CloudinaryService _cloudinaryService;

        public ProductQuantityController(DataContext context, CloudinaryService cloudinaryService)
        {
            _dataContext = context;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductQuantitiesByProductId(int productId)
        {
            try
            {
                var productQuantity = await _dataContext.ProductQuantities.Where(pq => pq.ProductId == productId).ToListAsync();
                return Ok(new
                {
                    Success = true,
                    Data = productQuantity
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProducrQuantity(int id)
        {
            try
            {
                var productQuantity = await _dataContext.ProductQuantities.FindAsync(id);

                 _dataContext.ProductQuantities.Remove(productQuantity);
                await _dataContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = " Xóa thành công số lượng sản phẩm"
                });

            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }
        [HttpPost("{productId}")]
        public async Task<IActionResult> ProductQuantity(int productId, ProductQuantityModel productQuantityl)
        {
            try
            {
                var product = await _dataContext.Products.FindAsync(productId);
                if (product == null)
                {
                    return NotFound(new { message = "Sản phẩm không tồn tại." });
                }

              
                if (productQuantityl == null || productQuantityl.Quantity <= 0)
                {
                    return BadRequest(new { message = "Số lượng không hợp lệ." });
                }

              
                productQuantityl.ProductId = productId;
                productQuantityl.DateCreated = DateTime.Now;

               
                _dataContext.ProductQuantities.Add(productQuantityl);
                await _dataContext.SaveChangesAsync();

                
                return Ok(new { id = productQuantityl.Id, productQuantityl });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }


    }
}
