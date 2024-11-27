namespace Authorization.Domain.Models;
public sealed record RegisterUserModel(string Username, string Email, string Password, string ConfirmPassword) { }
