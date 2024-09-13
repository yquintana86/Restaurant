using Application.Abstractions.Messaging;

namespace Application.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(string Name, string? Theme, string? Description, int WaiterId) : ICommand;
