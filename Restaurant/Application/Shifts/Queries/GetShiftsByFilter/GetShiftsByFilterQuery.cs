using Application.Abstractions.Messaging;
using SharedLib.Models.Common;

namespace Application.Shifts.Queries.GetShiftsByFilter;

public sealed class GetShiftsByFilterQuery : PagingFilter, IPagedQuery<ShiftResponse>
{
    public string? Name { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
}

