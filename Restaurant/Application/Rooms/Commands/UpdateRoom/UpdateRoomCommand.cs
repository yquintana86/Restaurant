using Application.Abstractions.Messaging;

namespace Application.Rooms.Commands.UpdateRoom;

public sealed record UpdateRoomCommand(int Id, string? Theme, string? Description, int WaiterId) : ICommand;
