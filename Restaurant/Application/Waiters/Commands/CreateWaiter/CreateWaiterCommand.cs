using Application.Abstractions.Messaging;

namespace Application.Waiters.Commands.CreateWaiter;

public sealed record CreateWaiterCommand(string FirstName, string LastName, decimal Salary, int ShiftId) : ICommand;

