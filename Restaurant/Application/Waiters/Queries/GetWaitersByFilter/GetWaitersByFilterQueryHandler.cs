using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Waiters.Queries.GetWaitersByFilter;

internal sealed class GetWaitersByFilterQueryHandler : IPagedQueryHandler<GetWaitersByFilterQuery, WaiterResponse>
{
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<GetWaitersByFilterQuery> _logger;
    public GetWaitersByFilterQueryHandler(IWaiterRepository waiterRepository, ILogger<GetWaitersByFilterQuery> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
    }

    public async Task<PagedResult<WaiterResponse>> Handle(GetWaitersByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _waiterRepository.SearchByFilterAsync(request, cancellationToken);

            var paged = new PagedResult<WaiterResponse>()
            {
                Currentpage = result.Currentpage,
                HasNextPage = result.HasNextPage,
                ItemCount = result.ItemCount,
                PageCount = result.PageCount,
                PageSize = result.PageSize,
                TotalItemCount = result.TotalItemCount,
                Results = result.Results?.Select(r => new WaiterResponse
                (
                    r.Id,
                    r.FirstName,
                    r.LastName,
                    r.Salary,
                    r.ShiftId,
                    new Shifts.Queries.ShiftResponse
                    {
                      Id = r.Shift.Id,
                      Name = r.Shift.Name,
                      Begin = r.Shift.StartTime,
                      End = r.Shift.EndTime
                    }
                )).ToList()
            };
            return paged;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception has occurred: {}", ex.Message);
            return new PagedResult<WaiterResponse>(new ApiOperationError(ex.GetType().Name, ex.Message,ApiErrorType.Failure));
        }
    }
}

