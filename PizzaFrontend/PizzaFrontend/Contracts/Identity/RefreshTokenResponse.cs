namespace PizzaFrontend.Contracts.Identity;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn
);
