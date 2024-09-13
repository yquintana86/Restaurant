using Application.Abstractions.Messaging;

namespace Application.Shifts.Commands.UpdateShift;

public sealed record UpdateShiftCommand(int Id, string Name, DateTime Begin, DateTime End) : ICommand;
