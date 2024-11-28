namespace Authorization.Domain.Models;

public sealed record LoginUserResult(string Username, string Email, string Token) { }