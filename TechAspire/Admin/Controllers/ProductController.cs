using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
	//[Authorize(Roles = "Admin")]
	public class ProductController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly CloudinaryService _cloudinaryService;

        public ProductController(DataContext context, CloudinaryService cloudinaryService)
        {
            _dataContext = context;
            _cloudinaryService = cloudinaryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _dataContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .ToListAsync();

                return Ok(new
                {
                    Success = true,
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                product.Images ??= new List<string>();

                if (product.ImageUploads != null && product.ImageUploads.Count > 0)
                {
                    foreach (var file in product.ImageUploads)
                    {
                        var imageUrl = await _cloudinaryService.UploadImageAsync(file);
                        product.Images.Add(imageUrl);
                    }
                }

                var newProduct = new ProductModel
                {
                    Name = product.Name,
                    Decription = product.Decription,
                    Price = product.Price,
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
					DiscountPercentage = product.DiscountPercentage,


					Sold = product.Sold,
                    Images = product.Images
                };

                _dataContext.Products.Add(newProduct);
                await _dataContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Data = newProduct
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(ProductModel product, int id)
        {


            try
            {

                var existingProduct = await _dataContext.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = "Sản phẩm không tồn tại" });
                }

                existingProduct.Name = product.Name;
                existingProduct.Decription = product.Decription;
                existingProduct.Price = product.Price;
                existingProduct.BrandId = product.BrandId;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.DiscountPercentage = product.DiscountPercentage;

				existingProduct.Sold = product.Sold;


                if (product.ImageUploads != null && product.ImageUploads.Count > 0)
                {
                    // Thêm ảnh mới vào mảng ảnh cũ
                    foreach (var file in product.ImageUploads)
                    {
                        var imageUrl = await _cloudinaryService.UploadImageAsync(file);
                        existingProduct.Images.Add(imageUrl);
                    }
                }
                if (product.Price <= 0 || product.Sold < 0)
                {
                    return BadRequest(new { message = "Giá hoặc số lượng không hợp lệ" });
                }

                _dataContext.Products.Update(existingProduct);
                await _dataContext.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Cập nhật sản phẩm thành công",
                    Data = existingProduct
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _dataContext.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = "Sản phẩm không tồn tại" });
                }


                foreach (var imageUrl in product.Images)
                {
                    await DeleteImageFromCloudinary(imageUrl);
                }

                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();

                return Ok(new { Success = true, message = "Sản phẩm đã được xóa thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa sản phẩm: {ex.Message}" });
            }
        }

        [HttpDelete("DeleteImage")]
        public async Task<IActionResult> DeleteImage([FromQuery] string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return BadRequest(new { message = "URL ảnh không hợp lệ" });
                }

                await DeleteImageFromCloudinary(imageUrl);
                return Ok(new { Success = true, message = "Ảnh đã được xóa thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa ảnh: {ex.Message}" });
            }
        }

        private async Task DeleteImageFromCloudinary(string imageUrl)
        {
            try
            {

                var publicId = imageUrl.Split('/').Last().Split('.').First();


                var result = await _cloudinaryService.DestroyAsync(publicId);

                if (result.Result != "ok")
                {
                    throw new Exception($"Không thể xóa ảnh: {imageUrl}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa ảnh: {ex.Message}");
            }
        }

    }
}
