using Application.Abstractions.Messaging;

namespace Application.Shifts.Commands.CreateShift
{
    public sealed record CreateShiftCommand(string Name, DateTime Begin, DateTime End) : ICommand;

}
