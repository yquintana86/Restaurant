using Application.Abstractions.Messaging;

namespace Application.Rooms.Queries.GetRoomById;

public sealed record GetRoomByIdQuery(int Id) : IQuery<GetRoomResponse>;
