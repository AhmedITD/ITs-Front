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

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
            .WithMessage("Min price must be non-negative.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue)
            .WithMessage("Max price must be non-negative.");

        RuleFor(x => x)
            .Must(x => !x.StartDateFrom.HasValue || !x.StartDateTo.HasValue || x.StartDateFrom <= x.StartDateTo)
            .WithMessage("From date must be before or equal to to date.");

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("Min price must be less than or equal to max price.");
    }
}
