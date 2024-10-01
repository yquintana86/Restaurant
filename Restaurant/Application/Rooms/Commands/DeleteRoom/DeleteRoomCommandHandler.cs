
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib;
using SharedLib.Models.Common;

namespace Application.Rooms.Commands.DeleteRoom;

internal sealed class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<DeleteRoomCommandHandler> _logger;

    public DeleteRoomCommandHandler(IRoomRepository roomRepository, ILogger<DeleteRoomCommandHandler> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var room = await _roomRepository.SearchByIdAsync(command.id,cancellationToken);
            if (room is null)
                return ApiOperationResult.Fail(RoomError.InvalidId(command.id));

            if (room.Tables is not null && room.Tables.Any())
                return ApiOperationResult.Fail(RoomError.RoomWithTables(room.Name));

            await _roomRepository.DeleteAsync(command.id);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name,ex.Message, ApiErrorType.Failure));
        }
    }
}
