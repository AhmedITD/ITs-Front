using FluentValidation;
using RentARide.Application.DTOs.requests.SuperQi;

namespace RentARide.Application.Validators.SuperQi;

public class SuperQiAgreementPayRequestValidator : AbstractValidator<SuperQiAgreementPayRequest>
{
    public SuperQiAgreementPayRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}
