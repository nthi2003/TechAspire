﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TechAspire.Models;

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

    public int Sold { get; set; }

    public CategoryModel? Category { get; set; }
    public BrandModel? Brand { get; set; }

    public List<string> Images { get; set; } = new List<string>();


    [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải từ 0 đến 100")]
	public decimal DiscountPercentage { get; set; }
	[NotMapped]
	public decimal DiscountedPrice => Price - (Price * DiscountPercentage / 100);


	[NotMapped]
 
    public List<IFormFile>? ImageUploads { get; set; }
}
