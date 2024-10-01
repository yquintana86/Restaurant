using SharedLib;
using SharedLib.Models.Common;

namespace Application.RoomTable.Queries;

public class GetRoomTableResponse
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public RoomTableStatusType Status { get; init; }
    public int TotalQty { get; init; }
    public int? WaiterId { get; set; }
    public string? WaiterFullName { get; set; }
}
