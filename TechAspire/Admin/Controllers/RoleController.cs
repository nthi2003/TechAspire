using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TechAspire.Admin.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet("GetAllRole")]
        public IActionResult GetAllRole()
        {
            try
            {
                var roles = _roleManager.Roles.Select(role => new
                {
                    role.Id,
                    role.Name
                }).ToList();
                return Ok(roles);
            }
            catch
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách vai trò" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string Name)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(Name);
                if (roleExist)
                {
                    return BadRequest(new { message = "Vai trò đã tồn tại" });
                }
                var role = new IdentityRole(Name);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new { role, message = "Vai trò đã được tạo thành công" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, string newRoleName)
        {
            try
            {


                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return BadRequest("Vai trò không tồn tại");
                }
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Vai trò đã được cập nhật thành công" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return BadRequest("Vai trò không tồn tại");
                }
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Vai trò được xóa thành công" });
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }

        }
    }
}
