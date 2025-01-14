﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
    public class WishListModel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
		public string Email { get; set; }

		[ForeignKey("ProductId")]

        public ProductModel Product { get; set; }
    }
}
