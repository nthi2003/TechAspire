using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly CloudinaryService _cloudinaryService;

        public ProductController(DataContext context, CloudinaryService cloudinaryService)
        {
            _dataContext = context;
            _cloudinaryService = cloudinaryService;
        }
        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                var product = await _dataContext.Products
                   .Where(p => p.Name.Contains(searchTerm) || p.Decription.Contains(searchTerm))
                   .ToListAsync();
                return Ok(new
                {
                    Success = true,
                    Keyword = searchTerm,
                    Data = product
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa ảnh: {ex.Message}");
            }
        }

  
        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsProduct(int id)
        {
            try
            {
                var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                
                var relatedProducts = await _dataContext.Products
                    .Where(p => p.Id != product.Id && (p.CategoryId == product.CategoryId || p.BrandId == product.BrandId))
                    .Take(4)
                    .ToListAsync();
                return Ok(new
                {
                    Success = true,
                    DetailProduct = product,
                    RelatedProducts = relatedProducts
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa ảnh: {ex.Message}");
            }
        }
    }
}
