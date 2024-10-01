using Application.Abstractions.Messaging;
using SharedLib;
using SharedLib.Models.Common;

namespace Application.RoomTable.Queries.GetTablesbyFilter;

public class GetRoomTablesByFilterQuery : PagingFilter, IPagedQuery<GetRoomTableResponse>
{
    public int? RoomId { get; init; }
    public int? WaiterId { get; init; }
    public RoomTableStatusType? Status { get; init; }
    public int? TotalQtyFrom { get; init; }
    public int? TotalQtyTo { get; init; }
}
