using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        [HttpGet("GetProductsByBrand")]
   
        public async Task<IActionResult> GetProductsByBrand(int brandId)
        {
            try
            {
                var brand = await _dataContext.Brands.FirstOrDefaultAsync(b => b.Id == brandId);
                if (brand == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Không tìm thấy thương hiệu"
                    });
                }
                var product = await _dataContext.Products
                    .Where(p => p.BrandId == brandId)
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
