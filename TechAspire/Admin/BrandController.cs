
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetBrands ()
        {
           try
            {
                var brand = await _dataContext.Brands.OrderBy(b => b.Id).ToListAsync();
                return Ok(new
                {
                    Success = true,
                    Data = brand
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateBrand(BrandModel brand)
        {
            try
            {
                
                var exitsbrand = await _dataContext.Brands.AnyAsync(b => b.Name == brand.Name);

                if (exitsbrand) // Nếu thương hiệu đã tồn tại
                {
                    return BadRequest(new { Success = false, Message = "Thương hiệu đã tồn tại." });
                }
                var createbrand = await _dataContext.AddAsync(brand);
                await _dataContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                    Data = brand,
                    message = "Tạo thương hiệu thành công"
                });



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] int id, [FromBody] BrandModel brand)
        {
            try
            {
                var existingBrand = await _dataContext.Brands.FindAsync(id);

                if (existingBrand == null)
                {
                    return BadRequest(new { Success = false, Message = "Thương hiệu không tồn tại." });
                }

              
                existingBrand.Name = brand.Name;
                existingBrand.Description = brand.Description;

              
                _dataContext.Brands.Update(existingBrand);
                await _dataContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Data = brand,
                    Message = "Cập nhật thương hiệu thành công"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                var brand = await _dataContext.Brands.FindAsync(id);
                if (brand == null)
                {
                    return BadRequest(new
                    {
                        Success = true,
                        Message = "Thương hiệu không tồn tại."
                    });
                }
                 _dataContext.Brands.Remove(brand);
                await _dataContext.SaveChangesAsync();
                return Ok(new
                {
                    Success = true,
                   
                    Message = " Xóa thương hiệu thành công"
                });



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

    }
}
