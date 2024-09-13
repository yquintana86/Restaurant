using Application.Abstractions.Messaging;

namespace Application.Waiters.Commands.DeleteWaiter;

public record class DeleteWaiterCommand(int Id) : ICommand;



