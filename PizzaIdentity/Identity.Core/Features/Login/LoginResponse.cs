namespace Identity.Core.Features.Login;

public sealed record LoginResponse(
    string Token, 
    string RefreshToken, 
    DateTime ExpiresAt);
