// Services/Interfaces/IAuthService.cs

using pr_srv_names.Dtos;

namespace pr_srv_names.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync(string userId, string? refreshToken = null);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto); // Старый метод
        Task<AuthResponseDto> RefreshTokenFromCookieAsync(string refreshToken); // Новый метод
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
    }
}