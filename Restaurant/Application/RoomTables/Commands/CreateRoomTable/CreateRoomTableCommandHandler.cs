using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.RoomTable.Commands.CreateTable;

internal class CreateRoomTableCommandHandler : ICommandHandler<CreateRoomTableCommand>
{
    private readonly IRoomTableRepository _tableRepository;
    private readonly ILogger<CreateRoomTableCommand> _logger;

    public CreateRoomTableCommandHandler(IRoomTableRepository tableRepository, ILogger<CreateRoomTableCommand> logger)
    {
        _tableRepository = tableRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(CreateRoomTableCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var table = new Domain.Entities.RoomTable
            {
                TotalQty = command.TotalQty,
                RoomId = command.RoomId,
                WaiterId = command.WaiterId
            };

            await _tableRepository.CreateAsync(table);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}
