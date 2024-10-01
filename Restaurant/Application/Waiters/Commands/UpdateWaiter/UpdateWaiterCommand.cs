using Application.Abstractions.Messaging;

namespace Application.Waiters.Commands.UpdateWaiter;

public record class UpdateWaiterCommand(int Id, string FirstName, string LastName, decimal Salary, DateTime Start) : ICommand;
