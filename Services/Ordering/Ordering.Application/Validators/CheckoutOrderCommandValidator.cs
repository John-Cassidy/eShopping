using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("{Username} is required")
            .NotNull()
            .MaximumLength(70)
            .WithMessage("{Username} must not exceed 70 characters");
        RuleFor(o => o.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("{FirstName} is required");
        RuleFor(o => o.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("{LastName} is required");
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithMessage("{EmailAddress} is required");
        RuleFor(x => x.TotalPrice)
            .NotEmpty()
            .WithMessage("{TotalPrice} is required")
            .GreaterThan(valueToCompare: -1)
            .WithMessage("{TotalPrice} must be greater than zero");
        // RuleFor(x => x.CardName)
        //     .NotEmpty()
        //     .WithMessage("Card name is required");
        // RuleFor(x => x.CardNumber)
        //     .NotEmpty()
        //     .WithMessage("Card number is required");
        // RuleFor(x => x.Expiration)
        //     .NotEmpty()
        //     .WithMessage("Card expiration is required");
        // RuleFor(x => x.CVV)
        //     .NotEmpty()
        //     .WithMessage("Card CVV is required")
        //     .Length(3)
        //     .WithMessage("Card CVV must be 3 characters")
        //     .Matches("[0-9]")
        //     .WithMessage("Card CVV must be numeric");
    }
}
