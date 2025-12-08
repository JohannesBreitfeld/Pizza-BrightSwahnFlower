namespace Identity.Core.Features;

public sealed record LoginResponse(
    string Token, 
    string RefreshToken, 
    DateTime ExpiresAt);
