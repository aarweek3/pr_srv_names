// Dtos/RoleDtos.cs
using System.ComponentModel.DataAnnotations;

namespace pr_srv_names.Dtos
{
    // DTO для создания роли
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Название роли обязательно")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    // DTO для обновления роли
    public class UpdateRoleDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    // DTO для отображения роли
    public class RoleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UsersCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // DTO для назначения ролей пользователю
    public class AssignRolesDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Укажите хотя бы одну роль")]
        public List<string> RoleNames { get; set; } = new();
    }

    // DTO для пользователя с ролями
    public class UserWithRolesDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}