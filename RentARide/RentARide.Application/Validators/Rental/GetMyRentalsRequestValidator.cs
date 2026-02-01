using FluentValidation;
using RentARide.Application.DTOs.Requests.RentalRequest;

namespace RentARide.Application.Validators.Rental;

public class GetMyRentalsRequestValidator : AbstractValidator<GetMyRentalsRequest>
{
    public GetMyRentalsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
