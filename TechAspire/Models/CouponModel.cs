using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên giảm giá")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu mô tả")]
        public string Description { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateExpired { get; set; }


        [Required(ErrorMessage = "Yêu cầu nhập số lượng giảm giá")]
        public int Quantity { get; set; }

        public int Status { get; set; }
    }
}
