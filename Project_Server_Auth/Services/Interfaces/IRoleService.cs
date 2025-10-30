// Services/Interfaces/IRoleService.cs

using pr_srv_names.Dtos;

namespace pr_srv_names.Services.Interfaces
{
    public interface IRoleService
    {
        // CRUD операции с ролями
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(string roleId);
        Task<RoleDto?> GetRoleByNameAsync(string roleName);
        Task<bool> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<bool> UpdateRoleAsync(string roleId, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteRoleAsync(string roleId);

        // Управление ролями пользователей
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> AssignRolesToUserAsync(string userId, List<string> roleNames);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<bool> RemoveAllUserRolesAsync(string userId);

        // Получение пользователей в роли
        Task<List<UserWithRolesDto>> GetUsersInRoleAsync(string roleName);

        // Проверки
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
    }
}