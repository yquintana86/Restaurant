using FluentValidation;

namespace Application.Waiters.Commands.UpdateWaiter;

public sealed class UpdateWaiterCommandValidation : AbstractValidator<UpdateWaiterCommand>
{
    public UpdateWaiterCommandValidation()
    {
        RuleFor(w => w.Id).GreaterThan(0);
        RuleFor(w => w.FirstName).NotNull().NotEmpty().MinimumLength(3);
        RuleFor(w => w.LastName).NotNull().NotEmpty().MinimumLength(3);
        RuleFor(w => w.Salary).GreaterThan(1);
        RuleFor(w => w.Start.Year).GreaterThanOrEqualTo(2020);
    }
}


