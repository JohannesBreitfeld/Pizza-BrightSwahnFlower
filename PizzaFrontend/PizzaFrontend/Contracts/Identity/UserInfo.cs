namespace PizzaFrontend.Contracts.Identity;

public sealed record UserInfo(
    string Username,
    string[] Roles,
    Dictionary<string, string> Claims
);
