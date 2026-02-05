using FluentValidation;
using RentARide.Application.DTOs.requests.VehicleRequest;

namespace RentARide.Application.Validators.Vehicle;

public class CreateVehicleTypeRequestValidator : AbstractValidator<CreateVehicleTypeRequest>
{
    public CreateVehicleTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
