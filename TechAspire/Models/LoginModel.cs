using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechAspire.Models
{
    public class LoginModel : IdentityUser
    {

        

        [Required(ErrorMessage = "Vui Lòng nhập email")]

        public string Email { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập password")]

        public string Password { get; set; }
       
    }
}
