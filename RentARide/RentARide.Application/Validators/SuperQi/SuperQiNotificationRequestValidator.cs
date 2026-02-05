using FluentValidation;
using RentARide.Application.DTOs.requests.SuperQi;

namespace RentARide.Application.Validators.SuperQi;

public class SuperQiNotificationRequestValidator : AbstractValidator<SuperQiNotificationRequest>
{
    public SuperQiNotificationRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");
    }
}
