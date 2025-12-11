namespace PizzaFrontend.Contracts.Identity;

public sealed record LoginRequest(
    string Username,
    string Password
);
