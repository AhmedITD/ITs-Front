using FluentValidation;
using RentARide.Application.DTOs.Requests.AuthRequest;

namespace RentARide.Application.Validators.Auth;

public class AuthWithSuperQiRequestValidator : AbstractValidator<AuthWithSuperQiRequest>
{
    public AuthWithSuperQiRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("SuperQi auth token is required.");
    }
}
