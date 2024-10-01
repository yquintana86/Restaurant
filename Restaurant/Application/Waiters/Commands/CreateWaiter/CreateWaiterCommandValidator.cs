using FluentValidation;

namespace Application.Waiters.Commands.CreateWaiter;

public sealed class CreateWaiterCommandValidator : AbstractValidator<CreateWaiterCommand>
{
    public CreateWaiterCommandValidator()
    {
        RuleFor(w => w.FirstName).NotNull().NotEmpty().MinimumLength(3);
        RuleFor(w => w.LastName).NotNull().NotEmpty().MinimumLength(3);
        RuleFor(w => w.Salary).GreaterThan(1);
    }
}

