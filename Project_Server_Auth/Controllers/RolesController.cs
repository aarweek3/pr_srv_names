// Controllers/RolesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Dtos;
using pr_srv_names.Services.Interfaces;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;


        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(new { success = true, data = roles });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка ролей");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(roleId);
                if (role == null)
                    return NotFound(new { success = false, message = "Роль не найдена" });

                return Ok(new { success = true, data = role });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении роли {RoleId}", roleId);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _roleService.CreateRoleAsync(createRoleDto);
                if (!result)
                    return BadRequest(new { success = false, message = "Не удалось создать роль" });

                return Ok(new { success = true, message = "Роль успешно создана" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании роли");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _roleService.UpdateRoleAsync(roleId, updateRoleDto);
                if (!result)
                    return NotFound(new { success = false, message = "Роль не найдена" });

                return Ok(new { success = true, message = "Роль успешно обновлена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении роли {RoleId}", roleId);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(roleId);
                if (!result)
                    return BadRequest(new { success = false, message = "Не удалось удалить роль" });

                return Ok(new { success = true, message = "Роль успешно удалена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении роли {RoleId}", roleId);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            try
            {
                var roles = await _roleService.GetUserRolesAsync(userId);
                return Ok(new { success = true, data = roles });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении ролей пользователя {UserId}", userId);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRolesToUser([FromBody] AssignRolesDto assignRolesDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _roleService.AssignRolesToUserAsync(
                    assignRolesDto.UserId,
                    assignRolesDto.RoleNames);

                if (!result)
                    return BadRequest(new { success = false, message = "Не удалось назначить роли" });

                return Ok(new { success = true, message = "Роли успешно назначены" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при назначении ролей пользователю");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{roleName}/users")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            try
            {
                var users = await _roleService.GetUsersInRoleAsync(roleName);
                return Ok(new { success = true, data = users });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователей роли {RoleName}", roleName);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }
    }
}