using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Rooms.Commands.UpdateRoom;

public sealed class UpdateRoomCommandHandler : ICommandHandler<UpdateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<UpdateRoomCommandHandler> _logger;

    public UpdateRoomCommandHandler(IRoomRepository roomRepository, IWaiterRepository waiterRepository, ILogger<UpdateRoomCommandHandler> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
        _waiterRepository = waiterRepository;
    }

    public async Task<ApiOperationResult> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        try
        { 
            var roomById = await _roomRepository.SearchByIdAsync(request.Id, cancellationToken);
            if (roomById is null)
                return ApiOperationResult.Fail(RoomError.NotFound(request.Id));

            Waiter? waiter = await _waiterRepository.SearchByIdAsync(request.WaiterId, cancellationToken);

            if (waiter is null)
                return ApiOperationResult.Fail(RoomError.RoomWaiterInChargeNotFound(request.WaiterId));

            if (waiter.Room is not null && waiter.Room!.Id != request.Id)
                return ApiOperationResult.Fail(RoomError.WaiterInChargeWithRoom(waiter.GetFullName));

            var room = new Room
            {
                Id = request.Id,
                WaiterId = request.WaiterId,
                Theme = request.Theme,
                Description = request.Description
            };
            
            await _roomRepository.UpdateAsync(room);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name, ex.Message, ApiErrorType.Failure));
        }
    }
}
