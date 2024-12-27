namespace TechAspire.Models
{
	public class CartModel
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public AppUserModel User { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public ICollection<CartItemModel> Items { get; set; } = new List<CartItemModel>();

		public decimal TotalAmount => Items.Sum(item => item.Total);
	}

}
