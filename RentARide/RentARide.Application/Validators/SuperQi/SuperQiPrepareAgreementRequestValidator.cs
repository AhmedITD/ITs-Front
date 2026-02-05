using FluentValidation;
using RentARide.Application.DTOs.requests.SuperQi;

namespace RentARide.Application.Validators.SuperQi;

public class SuperQiPrepareAgreementRequestValidator : AbstractValidator<SuperQiPrepareAgreementRequest>
{
    public SuperQiPrepareAgreementRequestValidator()
    {
        RuleFor(x => x.ContractDescription)
            .NotEmpty().WithMessage("Contract description is required.");
    }
}
