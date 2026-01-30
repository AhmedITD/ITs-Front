using FluentValidation;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.LoanRequest;

namespace Library.Application.Validators;

public class CreateLoanRequestValidator : AbstractValidator<CreateLoanRequest>
{
    public CreateLoanRequestValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty()
            .WithMessage("The Book ID it is required")
            .When(x => x.BookId.GetType() != 0.GetType() && x.BookId > 0)
            .WithMessage("The Book ID must Bee a valid number");

    }
}