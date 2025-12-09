using Identity.Core.Domain;

namespace Identity.Core.Contracts;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken?> GetByUserIdAsync(string userId);
    Task RevokeAsync(string token);
    Task RevokeAllByUserIdAsync(string userId);
}
