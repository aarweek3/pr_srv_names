// Services/AuthService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Enums;
using DAL.Models;
using pr_srv_names.Dtos;
using pr_srv_names.Services.Interfaces;
using DAL.Models.AuthorizationModels;

namespace pr_srv_names.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto, string? ipAddress = null, string? userAgent = null)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                throw new InvalidOperationException("Email already exists");

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) throw new InvalidOperationException("Failed to create user");

            await _userManager.AddToRoleAsync(user, "User");
            
            // Создание дефолтных настроек для нового пользователя
            var defaultSettings = new UserSettings
            {
                UserId = user.Id
                // Остальные свойства получат дефолтные значения из модели
            };
            _context.UserSettings.Add(defaultSettings);
            await _context.SaveChangesAsync();
            
            var tokens = await GenerateTokensAsync(user, ipAddress, userAgent);
            var roles = (await _userManager.GetRolesAsync(user)).Distinct().ToList();

            return new AuthResponseDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresAt = tokens.ExpiresAt,
                User = MapToUserProfileDto(user, roles)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? userAgent = null)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !user.IsActive) throw new UnauthorizedAccessException("Unauthorized");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Unauthorized");

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var tokens = await GenerateTokensAsync(user, ipAddress, userAgent);
            var roles = (await _userManager.GetRolesAsync(user)).Distinct().ToList();

            return new AuthResponseDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresAt = tokens.ExpiresAt,
                User = MapToUserProfileDto(user, roles)
            };
        }

        public async Task<bool> LogoutAsync(string userId, string? refreshToken = null)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);
                if (session != null) { session.IsRevoked = true; await _context.SaveChangesAsync(); }
            }
            return true;
        }

        public async Task<AuthResponseDto> RefreshTokenFromCookieAsync(string refreshToken, string? ipAddress = null, string? userAgent = null)
        {
            var session = await _context.UserSessions.Include(s => s.User).FirstOrDefaultAsync(s => s.RefreshToken == refreshToken && !s.IsRevoked);
            if (session == null || session.ExpiresAt <= DateTime.UtcNow) throw new UnauthorizedAccessException("Invalid refresh token");

            session.IsRevoked = true;
            var tokens = await GenerateTokensAsync(session.User, ipAddress, userAgent);
            var roles = (await _userManager.GetRolesAsync(session.User)).Distinct().ToList();
            return new AuthResponseDto { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken, ExpiresAt = tokens.ExpiresAt, User = MapToUserProfileDto(session.User, roles) };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto) => await RefreshTokenFromCookieAsync(dto.RefreshToken);

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            return result.Succeeded;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            var roles = (await _userManager.GetRolesAsync(user)).Distinct().ToList();
            
            _logger.LogInformation("GetUserProfile: UserId={UserId}, Email={Email}, IsExternal={IsExternal}, Provider={Provider}", 
                userId, user.Email, user.IsExternalAccount, user.ExternalProvider ?? "null");
            
            return MapToUserProfileDto(user, roles);
        }

        private async Task<TokenResult> GenerateTokensAsync(ApplicationUser user, string? ipAddress, string? userAgent)
        {
            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var session = new UserSession
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                DeviceInfo = userAgent // Simple fallback
            };
            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();
            return new TokenResult { AccessToken = accessToken, RefreshToken = refreshToken, ExpiresAt = DateTime.UtcNow.AddMinutes(60) };
        }

        private UserProfileDto MapToUserProfileDto(ApplicationUser user, IList<string> roles)
        {
            var dto = new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email ?? "",
                Department = user.Department,
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                Roles = roles.Distinct().ToList(),
                IsExternalAccount = user.IsExternalAccount,
                ExternalProvider = user.ExternalProvider,
                ExternalId = user.ExternalId
            };

            _logger.LogInformation("MapToUserProfileDto: IsExternal={IsExternal}, Provider={Provider}", 
                dto.IsExternalAccount, dto.ExternalProvider ?? "null");

            return dto;
        }

        public async Task<bool> UnlinkExternalAsync(string userId, string provider)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (user.ExternalProvider?.ToLower() == provider.ToLower())
            {
                user.ExternalProvider = null;
                user.ExternalId = null;
                user.IsExternalAccount = false;
                
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName))
                user.LastName = dto.LastName;

            if (dto.Avatar != null)
                user.Avatar = dto.Avatar;

            if (dto.Department != null)
                user.Department = dto.Department;

            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<List<UserSessionDto>> GetUserSessionsAsync(string userId, bool includeHistory = false)
        {
            var query = _context.UserSessions.Where(s => s.UserId == userId);

            if (!includeHistory)
            {
                query = query.Where(s => !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
            }

            var sessions = await query
                .OrderByDescending(s => s.CreatedAt)
                .Take(50)
                .Select(s => new UserSessionDto
                {
                    Id = s.Id,
                    RefreshToken = s.RefreshToken,
                    ExpiresAt = s.ExpiresAt,
                    IsRevoked = s.IsRevoked,
                    RevokedAt = s.RevokedAt,
                    DeviceInfo = s.DeviceInfo,
                    IpAddress = s.IpAddress,
                    UserAgent = s.UserAgent,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();

            return sessions;
        }

        public async Task<bool> RevokeSessionAsync(string userId, int sessionId)
        {
            var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);
            if (session == null) return false;

            session.IsRevoked = true;
            session.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private class TokenResult { public string AccessToken { get; set; } = ""; public string RefreshToken { get; set; } = ""; public DateTime ExpiresAt { get; set; } }
    }
}