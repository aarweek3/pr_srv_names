// Services/Interfaces/ITokenService.cs
using System.Security.Claims;
using DAL.Models.AuthorizationModels;

namespace pr_srv_names.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(ApplicationUser user); // Изменить на Task<string>
        string GenerateRefreshToken();
        ClaimsPrincipal? GetClaimsFromToken(string token);
        bool ValidateToken(string token);
        DateTime GetTokenExpiration(string token);
        string? GetUserIdFromToken(string token);
    }
}