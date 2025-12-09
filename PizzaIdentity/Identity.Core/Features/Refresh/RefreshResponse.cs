namespace Identity.Core.Features.Refresh;

public sealed record RefreshResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt);
