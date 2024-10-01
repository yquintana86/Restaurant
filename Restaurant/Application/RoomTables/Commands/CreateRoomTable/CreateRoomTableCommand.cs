using Application.Abstractions.Messaging;

namespace Application.RoomTable.Commands.CreateTable;

public record CreateRoomTableCommand(int RoomId, int? WaiterId, int TotalQty) : ICommand;

