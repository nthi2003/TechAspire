using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
	public class CartItemModel
	{
		[Key]
		public int CartItemId { get; set; }  

		public int ProductId { get; set; } 
		public ProductModel Product { get; set; } 

		public string ProductName { get; set; } 
		public int Quantity { get; set; }
		public decimal Price { get; set; } 
		
		public decimal Total => Quantity * Price;

		public List<string> Images { get; set; } = new List<string>();

	
		public string UserId { get; set; }
		public AppUserModel User { get; set; } 
	}
}
