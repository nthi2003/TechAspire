using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        [HttpGet("GetProductsByCategory")]

        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Không tìm thấy danh mục"
                    });

                }
                var product = await _dataContext.Products
                    .Where(p => p.CategoryId == categoryId)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .OrderBy(p => p.Id)
                    .ToArrayAsync();
                return Ok(new
                {
                    Success = true,
                    Data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }

        }
    }
}
