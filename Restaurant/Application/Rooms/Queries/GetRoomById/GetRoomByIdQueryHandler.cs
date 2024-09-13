using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Rooms.Queries.GetRoomById;

internal sealed class GetRoomByIdQueryHandler : IQueryHandler<GetRoomByIdQuery, GetRoomResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<GetRoomByIdQueryHandler> _logger;

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository, ILogger<GetRoomByIdQueryHandler> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<GetRoomResponse>> Handle(GetRoomByIdQuery query, CancellationToken cancellationToken)
    {
        var requestId = query.Id;
        if (requestId <= 0)
            return ApiOperationResult.Fail<GetRoomResponse>(RoomError.InvalidId(requestId));

        try
        {
            var room = await _roomRepository.SearchByIdAsync(requestId, cancellationToken);

            if (room is null)
                return ApiOperationResult.Fail<GetRoomResponse>(RoomError.NotFound(requestId));

            if (room.Waiter is null)
                return ApiOperationResult.Fail<GetRoomResponse>(RoomError.RoomWithWaiterInChargeNull(room.Id));

            var response = new GetRoomResponse
                (
                    room.Id,
                    room.Name,
                    room.Theme,
                    room.Description,
                    room.WaiterId,
                    room.Waiter.GetFullName
                );
            return ApiOperationResult.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {}", ex.Message);
            return ApiOperationResult.Fail<GetRoomResponse>(new ApiOperationError(ex.GetType().Name, ex.Message, ApiErrorType.Failure));
        }
    }
}