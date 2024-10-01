using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib;
using SharedLib.Models.Common;

namespace Application.RoomTable.Commands.DeleteTable;

internal class DeleteRoomTableCommandHandler : ICommandHandler<DeleteRoomTableCommand>
{

    private readonly IRoomTableRepository _tableRepository;
    private readonly ILogger<DeleteRoomTableCommand> _logger;

    public DeleteRoomTableCommandHandler(IRoomTableRepository tableRepository, ILogger<DeleteRoomTableCommand> logger)
    {
        _tableRepository = tableRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteRoomTableCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var table = await _tableRepository.SearchByIdAsync(command.Id, command.RoomId, cancellationToken);
            if (table == null)
                return ApiOperationResult.Fail(TableError.TableNotFound(command.Id, command.RoomId));

            if (table.Status != RoomTableStatusType.Unreserved)
                return ApiOperationResult.Fail(TableError.DeleteTableWithReservation(command.Id, command.RoomId));

            await _tableRepository.DeleteAsync(command.Id, command.RoomId);

            return ApiOperationResult.Success();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}
