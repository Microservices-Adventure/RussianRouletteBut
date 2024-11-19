using Authorization.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Authorization.Domain.Validators;
public partial class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    private const int MinimumLengthUsername = 5;
    private const int MinimumLengthPassword = 8;

    public RegisterUserModelValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(MinimumLengthUsername).WithMessage($"Minimum length of username: {MinimumLengthUsername}");

        RuleFor(user => user.Email)
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(user => user.Password)
            .MinimumLength(8).WithMessage($"Minimum length of password: {MinimumLengthPassword}")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit")
            .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter")
            .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }

    private bool ContainDigit(string password)
    {
        return DigitRegex().IsMatch(password);
    }

    private bool ContainLowercase(string password)
    {
        return LowercaseRegex().IsMatch(password);
    }

    private bool ContainUppercase(string password)
    {
        return UppercaseRegex().IsMatch(password);
    }

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex LowercaseRegex();
    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex UppercaseRegex();
    [GeneratedRegex(@"\d")]
    private static partial Regex DigitRegex();
}
