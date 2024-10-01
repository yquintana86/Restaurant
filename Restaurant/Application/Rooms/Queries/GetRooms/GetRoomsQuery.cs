using Application.Abstractions.Messaging;
using SharedLib.Models.Common;

namespace Application.Rooms.Queries.GetRooms;

public record GetRoomsQuery() : IQuery<List<SelectItem>>;
