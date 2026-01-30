using FluentValidation;
using Library.Application.DTOs.requests.AuthRequest;

namespace Library.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty()
            .WithMessage("The Email it is required")
            .EmailAddress()
            .WithMessage("The Email must be a valid email")
            .MaximumLength(50)
            .WithMessage("The Email must not exceed 50 characters");
        // .MustAsync(async (email, currentUserId, cancellationToken) =>
        //
        // )
        // .WithMessage("The Email is already registered !!");
            
        RuleFor(x => x.password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Your password must be at least 8 characters long.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one special character (!?*.).");
    }
}
