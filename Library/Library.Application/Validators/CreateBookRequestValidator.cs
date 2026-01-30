using FluentValidation;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.BookRequest;

namespace Library.Application.Validators;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("The Title it is required")
            .MaximumLength(50)
            .WithMessage("The Title must not exceed 50 characters");
    }
}