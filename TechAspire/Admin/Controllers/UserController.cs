using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAspire.Models;

namespace TechAspire.Admin.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;

        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var UserWithRole = new List<object>();
                foreach (var user in users)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    UserWithRole.Add(new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.PhoneNumber,
                        Roles = role
                    });
                }
                return Ok(UserWithRole);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(AppUserModel appUser, string role)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(appUser.Email);

                if (existingUser != null)
                {
                    return BadRequest("Tài khoản đã tồn tại, vui lòng đăng kí tài khoản khác");

                }


                var user = new AppUserModel
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    PhoneNumber = appUser.PhoneNumber,

                };
                var result = await _userManager.CreateAsync(user, appUser.PasswordHash);
                if (!result.Succeeded)
                {
                    return BadRequest("Không thể tạo tài khoản vui lòng thử lại");
                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest("Vai trò người dùng không tồn tại");
                }
                await _userManager.AddToRoleAsync(user, role);

                return Ok(new
                {
                    User = new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.PhoneNumber,
                        Roles = role
                    },
                    message = "Tạo tài khoản thành công"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(string id, string role)
        {
            try
            {
              
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return BadRequest("Người dùng không tồn tại");
                }

             
                if (!string.IsNullOrEmpty(role))
                {
                  
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        return BadRequest("Vai trò không tồn tại");
                    }

                 
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    foreach (var currentRole in currentRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(user, currentRole);
                    }

                    
                    await _userManager.AddToRoleAsync(user, role);
                }

               
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Không thể cập nhật thông tin người dùng, vui lòng thử lại");
                }

                // Trả về kết quả
                return Ok(new
                {
                    User = new
                    {
                        Roles = role
                    },
                    message = "Cập nhật vai trò thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }


        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return BadRequest("Người dùng không tồn tại");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Xóa tài khoản thất bại");
                }
                return Ok(new
                {
                    message = "Xóa tài khoản thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }

        }




    }
}
