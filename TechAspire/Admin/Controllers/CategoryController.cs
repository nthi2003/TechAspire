
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            try
            {

                var category = await _dataContext.Categories.OrderBy(b => b.Id).ToListAsync();
                return Ok(new
                {
                    Success = true,
                    Data = category
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryModel category)
        {
            try
            {

                var exitscategory = await _dataContext.Categories.AnyAsync(b => b.Name == category.Name);

                if (exitscategory) // Nếu thương hiệu đã tồn tại
                {
                    return BadRequest(new { Success = false, Message = "Danh mục đã tồn tại." });
                }
                var createCategory = await _dataContext.AddAsync(category);
                await _dataContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Data = category,
                    message = "Tạo danh mục thành công"
                });



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryModel category)
        {
            try
            {
                var existingCategory = await _dataContext.Categories.FindAsync(id);

                if (existingCategory == null)
                {
                    return BadRequest(new { Success = false, Message = "Danh mục không tồn tại." });
                }


                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;


                _dataContext.Categories.Update(existingCategory);
                await _dataContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Data = category,
                    Message = "Cập danh mục thành công"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _dataContext.Categories.FindAsync(id);
                if (category == null)
                {
                    return BadRequest(new
                    {
                        Success = true,
                        Message = "Danh mục không tồn tại."
                    });
                }
                _dataContext.Categories.Remove(category);
                await _dataContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,

                    Message = " Xóa danh mục thành công"
                });



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpGet("GetProductsByCategory")]
        
        public async Task<IActionResult> GetProductsByCategory (int categoryId)
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
