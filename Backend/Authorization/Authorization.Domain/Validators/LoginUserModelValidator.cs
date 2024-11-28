using Authorization.Domain.Models;
using FluentValidation;

namespace Authorization.Domain.Validators;

public class LoginUserModelValidator : AbstractValidator<LoginUserModel>
{
    private const int MinimumLengthUsername = 5;
    
    public LoginUserModelValidator()
    {
        RuleFor(login => login.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(MinimumLengthUsername).WithMessage($"Minimum length of username: {MinimumLengthUsername}");

        RuleFor(login => login.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}