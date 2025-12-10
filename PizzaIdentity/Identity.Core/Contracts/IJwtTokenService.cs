using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Contracts;

public interface IJwtTokenService
{
    Task<string> GenerateJwtTokenAsync(IdentityUser user, UserManager<IdentityUser> userManager);
    string GenerateRefreshToken();
    DateTime GetTokenExpiration();
    int GetRefreshTokenExpirationDays();
}
