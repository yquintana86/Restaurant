using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;
using System.Globalization;

namespace Application.RoomTable.Queries.GetTablesbyFilter;

internal class GetRoomTablesByFilterQueryHandler : IPagedQueryHandler<GetRoomTablesByFilterQuery, GetRoomTableResponse>
{
    private readonly IRoomTableRepository _tableRepository;
    private readonly ILogger<GetRoomTableResponse> _logger;

    public GetRoomTablesByFilterQueryHandler(IRoomTableRepository tableRepository, ILogger<GetRoomTableResponse> logger)
    {
        _tableRepository = tableRepository;
        _logger = logger;
    }

    public async Task<PagedResult<GetRoomTableResponse>> Handle(GetRoomTablesByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var paged = await _tableRepository.SearchByFilterAsync(request, cancellationToken);

            var pagedResult = new PagedResult<GetRoomTableResponse>
            {
                Currentpage = paged.Currentpage,
                PageSize = paged.PageSize,
                HasNextPage = paged.HasNextPage,
                ItemCount = paged.ItemCount,
                TotalItemCount = paged.TotalItemCount,
                PageCount = paged.PageCount,
                Results = paged.Results!.Select(tbl => new GetRoomTableResponse
                {
                    Id = tbl.Id,
                    RoomId =  tbl.RoomId,
                    RoomName = tbl.Room?.Name ?? string.Empty,
                    WaiterId = tbl.WaiterId,
                    WaiterFullName = tbl.Waiter?.GetFullName ?? default,
                    Status = tbl.Status,
                    TotalQty = tbl.TotalQty
                }).ToList()

            };

            return pagedResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error has occurred:{}", ex.Message);
            return new PagedResult<GetRoomTableResponse>(new ApiOperationError(ex.GetType().Name, ex.Message, ApiErrorType.Failure));

        }
    }
}
