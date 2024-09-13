using Application.Abstractions.Messaging;

namespace Application.Waiters.Queries.GetWaiterById;

public sealed record GetWaiterByIdQuery(int Id) : IQuery<WaiterResponse>;
