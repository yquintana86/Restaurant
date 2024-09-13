using FluentValidation;

namespace Application.Shifts.Commands.DeleteShift;

public sealed class DeleteShiftCommandValidator : AbstractValidator<DeleteShiftCommand>
{
    public DeleteShiftCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0); 
    }
}

