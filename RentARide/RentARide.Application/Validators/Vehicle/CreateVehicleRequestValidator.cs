using FluentValidation;
using RentARide.Application.DTOs.Requests.VehicleRequest;

namespace RentARide.Application.Validators;

public class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequest>
{
    public CreateVehicleRequestValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(200).WithMessage("Model must not exceed 200 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, 2100).WithMessage("Year must be between 1900 and 2100.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
            .MaximumLength(20).WithMessage("License plate must not exceed 20 characters.");

        RuleFor(x => x.DailyPrice)
            .GreaterThan(0).WithMessage("Daily price must be greater than 0.");

        RuleFor(x => x.VehicleTypeId)
            .NotEmpty().WithMessage("Vehicle type ID is required.")
            .GreaterThan(0).WithMessage("Vehicle type ID must be greater than 0.");
        
        RuleFor(x=>x.LastMaintenanceDate)
            .LessThan(DateTime.UtcNow).WithMessage("Last maintenance date must be in the past.");
        
        RuleFor(x=>x.NextMaintenanceDue)
            .GreaterThan(x=>x.LastMaintenanceDate).WithMessage("Next maintenance due date must be after last maintenance date.");
    }
}
