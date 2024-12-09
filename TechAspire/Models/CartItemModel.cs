using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
    public class CartItemModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProdutName { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;

        public List<string> Images { get; set; } = new List<string>();

        public CartItemModel()
        {

        }
        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            ProdutName = product.Name;
            Price = product.Price;
            Quantity = 1;
            Images = product.Images;
        }
    }
}
