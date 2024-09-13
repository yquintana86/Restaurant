using FluentValidation;

namespace Application.Shifts.Commands.UpdateShift;

public sealed class UpdateShiftCommandValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftCommandValidator()
    {
        RuleFor(sh => sh.Id).GreaterThan(0);
        RuleFor(sh => sh.Name).NotEmpty();
        RuleFor(c => c.Begin).NotNull();
        RuleFor(c => c.End).NotNull();
        RuleFor(c => c.Begin).NotEqual(c => c.End);
    }
}
