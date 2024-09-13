
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
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

    public async Task<ApiOperationResult> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var roomExist = await _roomRepository.ExistIdAsync(request.id,cancellationToken);
            if (!roomExist)
                return ApiOperationResult.Fail(RoomError.InvalidId(request.id));

            await _roomRepository.DeleteAsync(request.id);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name,ex.Message, ApiErrorType.Failure));
        }
    }
}
