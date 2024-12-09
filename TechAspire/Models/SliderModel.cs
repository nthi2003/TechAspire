using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
    public class SliderModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên slider")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả")]
        public string Description { get; set; }

        public int? Status { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

        // Foreign Key
        public string? UserId { get; set; }


        public AppUserModel? User { get; set; }
    }
}
