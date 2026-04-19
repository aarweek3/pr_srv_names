// Services/RoleService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Dtos;
using pr_srv_names.Services.Interfaces;
using DAL.Models.AuthorizationModels;

namespace pr_srv_names.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleDtos = new List<RoleDto>();

            foreach (var role in roles)
            {
                var usersCount = (await _userManager.GetUsersInRoleAsync(role.Name!)).Count;
                roleDtos.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name!,
                    UsersCount = usersCount
                });
            }

            return roleDtos;
        }

        public async Task<RoleDto?> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null;

            var usersCount = (await _userManager.GetUsersInRoleAsync(role.Name!)).Count;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                UsersCount = usersCount
            };
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return null;

            var usersCount = (await _userManager.GetUsersInRoleAsync(role.Name!)).Count;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                UsersCount = usersCount
            };
        }

        public async Task<bool> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            if (await _roleManager.RoleExistsAsync(createRoleDto.Name))
            {
                _logger.LogWarning("Роль {RoleName} уже существует", createRoleDto.Name);
                return false;
            }

            var role = new IdentityRole(createRoleDto.Name);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                _logger.LogInformation("Роль {RoleName} успешно создана", createRoleDto.Name);
            }
            else
            {
                _logger.LogError("Ошибка создания роли: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result.Succeeded;
        }

        public async Task<bool> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            if (!string.IsNullOrEmpty(updateRoleDto.Name))
            {
                role.Name = updateRoleDto.Name;
            }

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            // Проверка системных ролей
            if (role.Name == "Admin" || role.Name == "User")
            {
                _logger.LogWarning("Попытка удаления системной роли {RoleName}", role.Name);
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> AssignRolesToUserAsync(string userId, List<string> roleNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Удаляем все текущие роли
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Добавляем новые роли
            var result = await _userManager.AddToRolesAsync(user, roleNames);
            return result.Succeeded;
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                _logger.LogWarning("Роль {RoleName} не существует", roleName);
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveAllUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<List<UserWithRolesDto>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var userDtos = new List<UserWithRolesDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserWithRolesDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Roles = roles.ToList()
                });
            }

            return userDtos;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}