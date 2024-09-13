using Application.Abstractions.Messaging;
using SharedLib.Models.Common;
using System.Collections.Generic;

namespace Application.Waiters.Queries.GetWaiters;

public sealed record GetWaitersQuery(bool IsRoomResponsable) : IQuery<List<SelectItem>>;
