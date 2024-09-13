using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Rooms.Queries.GetRoomsByFilter;

internal sealed class GetRoomsByFilterQueryHandler : IPagedQueryHandler<GetRoomsByFilterQuery, GetRoomResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<GetRoomsByFilterQueryHandler> _logger;
    public GetRoomsByFilterQueryHandler(IRoomRepository roomRepository, ILogger<GetRoomsByFilterQueryHandler> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<PagedResult<GetRoomResponse>> Handle(GetRoomsByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.WaiterId.HasValue && request.WaiterId.Value == 0)
                return new PagedResult<GetRoomResponse>(RoomError.InvalidId(request.WaiterId.Value));

            var paged = await _roomRepository.GetbyFilterAsync(request, cancellationToken);

            var pagedResponse = new PagedResult<GetRoomResponse>()
            {
                Currentpage = paged.Currentpage,
                PageSize = paged.PageSize,
                HasNextPage = paged.HasNextPage,
                ItemCount = paged.ItemCount,
                PageCount = paged.PageCount,
                TotalItemCount = paged.TotalItemCount,
                Results = paged.Results?.Select(r => new GetRoomResponse
                (
                    r.Id,
                    r.Name,
                    r.Theme,
                    r.Description,
                    r.WaiterId,
                    r.Waiter.GetFullName
                )).ToList()
            };

            return pagedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);
            return new PagedResult<GetRoomResponse>(new ApiOperationError(ex.GetType().Name,ex.Message,ApiErrorType.Failure));
        }
    }
}


