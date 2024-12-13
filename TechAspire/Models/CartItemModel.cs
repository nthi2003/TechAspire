using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
	public class CartItemModel
	{
		[Key]
		public int CartItemId { get; set; }  

		public int ProductId { get; set; }  // Foreign key for the product
		public ProductModel Product { get; set; }  // Navigation property for Product

		public string ProductName { get; set; }  // Optional: If you need to store the product name in the CartItem
		public int Quantity { get; set; }  // Quantity of the product in the cart
		public decimal Price { get; set; }  // Price of the product

		// Calculated property for the total price
		public decimal Total => Quantity * Price;

		public List<string> Images { get; set; } = new List<string>();  // Images for the product

		// Foreign key for the user
		public string UserId { get; set; }
		public AppUserModel User { get; set; }  // Navigation property for the user
	}
}
