using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TechAspire.Models;
using System.Text.Json.Serialization;

public class ProductModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập Tên Sản Phẩm")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập Mô tả Sản Phẩm")]
    public string Decription { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập Giá Sản Phẩm")]
    [Range(0.01, double.MaxValue)]
    [Column(TypeName = "NUMERIC(15, 2)")]
    public decimal Price { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một thương hiệu")]
    public int BrandId { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục")]
    public int CategoryId { get; set; }

    public int Quantity { get; set; }
    public int Sold { get; set; }

    // Không bắt buộc ánh xạ đến đối tượng Brand hoặc Category
    public CategoryModel? Category { get; set; }
    public BrandModel? Brand { get; set; }

    public List<string> Images { get; set; } = new List<string>();

    [NotMapped]
    [JsonIgnore] // lỗi bỏ thằng này khi respone trả về
    public List<IFormFile>? ImageUploads { get; set; }
}
