
using Application.Abstractions.Messaging;

namespace Application.Rooms.Commands.DeleteRoom;

public record class DeleteRoomCommand(int id): ICommand;
