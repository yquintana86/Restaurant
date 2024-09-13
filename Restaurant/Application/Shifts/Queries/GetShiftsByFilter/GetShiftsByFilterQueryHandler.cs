using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Shifts.Queries.GetShiftsByFilter;

internal sealed class GetShiftsByFilterQueryHandler : IPagedQueryHandler<GetShiftsByFilterQuery, ShiftResponse>
{
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<GetShiftsByFilterQueryHandler> _logger;

    public GetShiftsByFilterQueryHandler(IShiftRepository shiftRepository, ILogger<GetShiftsByFilterQueryHandler> logger)
    {
        _shiftRepository = shiftRepository;
        _logger = logger;
    }

    public async Task<PagedResult<ShiftResponse>> Handle(GetShiftsByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shiftRepository.SearchByFilterAsync(request, cancellationToken);

            var paged = new PagedResult<ShiftResponse>
            {
                Currentpage = result.Currentpage,
                HasNextPage = result.HasNextPage,
                TotalItemCount = result.TotalItemCount,
                PageSize = result.PageSize,
                ItemCount = result.ItemCount,
                PageCount = result.PageCount,
                Results = result.Results?.Select(sh => new ShiftResponse
                {
                    Id = sh.Id,
                    Name = sh.Name,
                    Begin = sh.StartTime,
                    End = sh.EndTime
                }).ToList()
            };

            return paged;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception ocurred: {}", ex.Message);
            return PagedResult<ShiftResponse>.WithError(new ApiOperationError(ex.GetType().Name, ex.Message,ApiErrorType.Failure));
        }
    }
}

