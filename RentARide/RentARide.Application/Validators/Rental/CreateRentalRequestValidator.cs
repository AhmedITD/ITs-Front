using FluentValidation;
using RentARide.Application.DTOs.requests.RentalRequest;

namespace RentARide.Application.Validators.Rental;

public class CreateRentalRequestValidator : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Vehicle ID must be greater than 0.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date must be in the future.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.")
            .GreaterThan(DateTime.UtcNow).WithMessage("End date must be in the future.")
            .GreaterThanOrEqualTo(x=>x.StartDate.AddHours(1)).WithMessage("The end date must be at least an hour after the start date.");
    }
}