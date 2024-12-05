
using Microsoft.AspNetCore.Identity;


using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TechAspire.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;
using TechAspire.Models;

namespace TechAspire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(appUser.Email);
                if(existingUser != null)
                {
                    return BadRequest("Email đã tồn tại");
                }
                var user = new AppUser
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    PhoneNumber = appUser.PhoneNumber,
                };
                var roleExist = await _roleManager.RoleExistsAsync("User");
                if(!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest("Không thể tạo role 'User'.");
                    }
                }

                var result = await _userManager.CreateAsync(user, appUser.PasswordHash);
                var receiver = appUser.Email;


                var subject = "Đăng nhập trên thiết bị thành công";
                var message = "Đăng nhập thành công , trải nghiệm dịch vụ nhé";

                await _emailSender.SendEmailAsync(receiver, subject, message);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await _userManager.AddToRoleAsync(user, "User");

                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken(user);



                return Ok(new { user, Token = token, RefreshToken = refreshToken });
            }
            catch 
            {
                return BadRequest("Lỗi");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {


                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user == null)
                {
                    return Unauthorized("Email hoặc mật khẩu sai");
                }


                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);


                if (!signInResult.Succeeded)
                {
                    return Unauthorized("Email hoặc mật khẩu sai");
                }
                if (user == null)
                {
                    return Unauthorized("Người dùng không tồn tại");
                }
                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken(user);
               
                    var receiver = loginModel.Email;
                    

                    var subject = "Đăng nhập trên thiết bị thành công";
                    var message = "Đăng nhập thành công , trải nghiệm dịch vụ nhé";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
               


                return Ok(new { user , Token = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        



        private string GenerateJwtToken(AppUser appUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", appUser.Id),
                new Claim(ClaimTypes.Role, "user") 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

  
        private string GenerateRefreshToken(AppUser appUser)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("id", appUser.Id),
        new Claim(ClaimTypes.Role, "user"), 
        new Claim("type", "refresh") 
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var refreshToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(7), // Thời gian sống của refreshToken có thể dài hơn JWT
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

       
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = tokenHandler.ReadToken(refreshToken) as JwtSecurityToken;
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // Không cần kiểm tra hết hạn vì refresh token sẽ không hết hạn ngay
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        
    }
}
