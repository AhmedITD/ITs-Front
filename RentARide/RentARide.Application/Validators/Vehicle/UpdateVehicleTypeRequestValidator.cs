using FluentValidation;
using RentARide.Application.DTOs.Requests.VehicleRequest;

namespace RentARide.Application.Validators;

public class UpdateVehicleTypeRequestValidator : AbstractValidator<UpdateVehicleTypeRequest>
{
    public UpdateVehicleTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
