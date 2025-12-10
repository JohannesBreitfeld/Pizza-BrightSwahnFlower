namespace PizzaFrontend.Contracts.Identity;

public sealed record UserInfo(
    string Email,
    string[] Roles,
    Dictionary<string, string> Claims
);
