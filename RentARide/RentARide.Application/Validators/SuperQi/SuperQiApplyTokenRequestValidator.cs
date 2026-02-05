using FluentValidation;
using RentARide.Application.DTOs.requests.SuperQi;

namespace RentARide.Application.Validators.SuperQi;

public class SuperQiApplyTokenRequestValidator : AbstractValidator<SuperQiApplyTokenRequest>
{
    public SuperQiApplyTokenRequestValidator()
    {
        RuleFor(x => x.AuthCode)
            .NotEmpty().WithMessage("Auth code is required.");
    }
}
