// Services/Interfaces/IAuthService.cs

using pr_srv_names.Dtos;

namespace pr_srv_names.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, string? ipAddress = null, string? userAgent = null);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? userAgent = null);
        Task<bool> LogoutAsync(string userId, string? refreshToken = null);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto); // Старый метод
        Task<AuthResponseDto> RefreshTokenFromCookieAsync(string refreshToken, string? ipAddress = null, string? userAgent = null); // Новый метод
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
        Task<bool> UnlinkExternalAsync(string userId, string provider);
        Task<bool> UpdateUserProfileAsync(string userId, UpdateUserDto dto);
        Task<List<UserSessionDto>> GetUserSessionsAsync(string userId, bool includeHistory = false);
        Task<bool> RevokeSessionAsync(string userId, int sessionId);
    }
}