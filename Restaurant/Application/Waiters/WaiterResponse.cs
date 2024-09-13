using Application.Shifts.Queries;

namespace Application.Waiters;

public sealed record WaiterResponse(int Id, string FirstName, string LastName, decimal Salary, int ShiftId, ShiftResponse Shift);

