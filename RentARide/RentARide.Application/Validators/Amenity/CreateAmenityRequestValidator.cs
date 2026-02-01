using FluentValidation;
using RentARide.Application.DTOs.Requests.AmenityRequest;

namespace RentARide.Application.Validators.Amenity;

public class CreateAmenityRequestValidator : AbstractValidator<CreateAmenityRequest>
{
    public CreateAmenityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
    }
}
