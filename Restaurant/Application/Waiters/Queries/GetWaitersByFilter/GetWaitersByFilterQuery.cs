using Application.Abstractions.Messaging;
using SharedLib.Models.Common;

namespace Application.Waiters.Queries.GetWaitersByFilter;

public sealed class GetWaitersByFilterQuery : PagingFilter, IPagedQuery<WaiterResponse>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal? SalaryFrom { get; set; }
    public decimal? SalaryTo { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}

