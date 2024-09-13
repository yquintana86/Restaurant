using Application.Abstractions.Messaging;
using SharedLib.Models.Common;

namespace Application.Rooms.Queries.GetRoomsByFilter;

public sealed class GetRoomsByFilterQuery : PagingFilter, IPagedQuery<GetRoomResponse>
{
    public string? Name { get; set; }
    public int? WaiterId { get; set; }
}


