using Application.Abstractions.Messaging;

namespace Application.RoomTable.Commands.DeleteTable;

public record DeleteRoomTableCommand(int Id, int RoomId) : ICommand;
