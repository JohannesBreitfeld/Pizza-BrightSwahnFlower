namespace PizzaFrontend.Contracts.Identity;

public sealed record RefreshTokenResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt
);
