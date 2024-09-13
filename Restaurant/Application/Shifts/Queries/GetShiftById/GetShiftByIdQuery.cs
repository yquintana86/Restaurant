using Application.Abstractions.Messaging;

namespace Application.Shifts.Queries.GetShiftById;

public sealed record GetShiftByIdQuery(int ShiftId) : IQuery<ShiftResponse>;





