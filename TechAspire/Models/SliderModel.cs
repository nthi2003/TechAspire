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
	

		public string? UserName { get; set; }  
		public string? UserId { get; set; }  
		public DateTime CreatedDate { get; set; }
		public List<string> Images { get; set; } = new List<string>();


		[NotMapped]
		public List<IFormFile>? ImageUploads { get; set; }
		public AppUserModel? User { get; set; }
	}

}
