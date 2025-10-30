// Services/Interfaces/IUserService.cs

using pr_srv_names.Dtos;

namespace pr_srv_names.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetUserByIdAsync(string userId);
        Task<UserProfileDto?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);
        Task<bool> DeactivateUserAsync(string userId);
        Task<bool> ActivateUserAsync(string userId);
        Task<PagedResponseDto<UserListItemDto>> GetUsersAsync(UserFilterDto filter);
        Task<UserStatisticsDto> GetUserStatisticsAsync();
        Task<bool> DeleteUserAsync(string userId);
        Task<List<UserSearchResultDto>> SearchUsersAsync(string query, int limit = 10);
    }
}