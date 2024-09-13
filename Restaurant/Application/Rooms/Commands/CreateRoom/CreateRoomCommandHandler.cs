using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Azure.Core;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Rooms.Commands.CreateRoom;

internal sealed class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<CreateRoomCommand> _logger;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IWaiterRepository waiterRepository, ILogger<CreateRoomCommand> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
        _waiterRepository = waiterRepository;
    }

    public async Task<ApiOperationResult> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exist = await _roomRepository.ExistNameAsync(request.Name, cancellationToken);
            if (exist)
                return ApiOperationResult.Fail(RoomError.RoomNameDuplicated(request.Name));

            Waiter? waiter = await _waiterRepository.SearchByIdAsync(request.WaiterId, cancellationToken);

            if (waiter is null)
                return ApiOperationResult.Fail(RoomError.RoomWaiterInChargeNotFound(request.WaiterId));

            if (waiter.Room is not null)
                return ApiOperationResult.Fail(RoomError.WaiterInChargeWithRoom(waiter.GetFullName));

            var room = new Room
            {
                Name = request.Name,
                WaiterId = request.WaiterId,
                Theme = request.Theme,
                Description = request.Description
            };

            await _roomRepository.CreateAsync(room, cancellationToken);

            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name, ex.Message, ApiErrorType.Failure));
        }

    }


}
