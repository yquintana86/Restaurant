namespace Application.Rooms.Queries;

public sealed record GetRoomResponse(int Id, string Name, string? Theme, string? Description, int WaiterId, string WaiterName);
