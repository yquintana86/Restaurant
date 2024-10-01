using Application.Abstractions.Messaging;
using SharedLib;

namespace Application.RoomTable.Commands.UpdateTable;

public record UpdateRoomTableCommand(int Id, int RoomId, RoomTableStatusType Status, int? WaiterId, int TotalQty) : ICommand;


