using FluentValidation;
using RentARide.Application.DTOs.requests.SuperQi;

namespace RentARide.Application.Validators.SuperQi;

public class SuperQiRefundRequestValidator : AbstractValidator<SuperQiRefundRequest>
{
    public SuperQiRefundRequestValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("Payment ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}
