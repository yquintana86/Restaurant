using Application.Abstractions.Messaging;

namespace Application.Shifts.Commands.DeleteShift;

public record DeleteShiftCommand(int Id) : ICommand;

