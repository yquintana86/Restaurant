namespace Application.Waiters;

public sealed record WaiterResponse(int Id, string FirstName, string LastName, decimal Salary, DateTime Start, DateTime? End, string? RoomName);

