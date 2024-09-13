using FluentValidation;

namespace Application.Shifts.Commands.CreateShift;

public sealed class CreateShiftCommandValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftCommandValidator()
    {
        RuleFor(c => c.Name).NotNull().NotEmpty().MinimumLength(3);
        RuleFor(c => c.Begin).NotNull();
        RuleFor(c => c.End).NotNull();
        RuleFor(c => c.Begin).NotEqual(c => c.End);
    }
}
