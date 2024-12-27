using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TechAspire.Models;

public class CartItemModel
{
	[Key]
	public int Id { get; set; }

	public int CartId { get; set; }

	[JsonIgnore]
	public CartModel Cart { get; set; }

	public int ProductId { get; set; }
	public ProductModel Product { get; set; }

	public int Quantity { get; set; }
	public decimal Price { get; set; }

	public decimal Total => Quantity * Price;
}
