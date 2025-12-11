namespace PizzaFrontend.Contracts.Identity;

public sealed record LoginResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt
);
