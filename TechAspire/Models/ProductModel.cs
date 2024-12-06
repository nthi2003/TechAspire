using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TechAspire.Models;

public class ProductModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập Tên Sản Phẩm")]
    public string Name { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một thương hiệu")]
    public int BrandId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục")]
    public int CategoryId { get; set; }

    public int Quantity { get; set; }
    public int Sold { get; set; }

    // Khai báo khóa ngoại
    
    public CategoryModel Category { get; set; }

    public BrandModel Brand { get; set; }

    public string Image { get; set; }
    [NotMapped]
    public IFormFile? ImageUpload { get; set; }
}
