using FluentValidation;

namespace Application.Waiters.Commands.DeleteWaiter;

public sealed class DeleteWaiterCommandValidator : AbstractValidator<DeleteWaiterCommand>
{
    public DeleteWaiterCommandValidator()
    {
        RuleFor(w => w.Id).GreaterThan(0);
    }
}



