﻿namespace Authorization.Domain.Models;
public class RegisterUserModel
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
