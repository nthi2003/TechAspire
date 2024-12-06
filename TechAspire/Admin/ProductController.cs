using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    

    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet] 
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _dataContext.Products.Include(p => p.Category).Include(p => p.Brand).ToListAsync();
                return Ok(new
                {
                    Success = true,
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct()
        {

        }

    }
}
