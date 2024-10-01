using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.RoomTable.Commands.UpdateTable;

internal class UpdateRoomTableCommandHandler : ICommandHandler<UpdateRoomTableCommand>
{
    private readonly IRoomTableRepository _roomTableRepository;
    private readonly ILogger<UpdateRoomTableCommand> _logger;


    public UpdateRoomTableCommandHandler(IRoomTableRepository tableRepository, ILogger<UpdateRoomTableCommand> logger)
    {
        _roomTableRepository = tableRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(UpdateRoomTableCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var table = await _roomTableRepository.SearchByIdAsync(command.Id, command.RoomId, cancellationToken);

            if (table is null)
                return ApiOperationResult.Fail(TableError.TableNotFound(command.Id, command.RoomId));

            var roomTable = new Domain.Entities.RoomTable
            {
                Id = command.Id,
                RoomId = command.RoomId,
                Status = command.Status,
                TotalQty = command.TotalQty,
                WaiterId = command.WaiterId
            };

            await _roomTableRepository.UpdateAsync(roomTable);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has ocurred: {}", ex.Message);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

