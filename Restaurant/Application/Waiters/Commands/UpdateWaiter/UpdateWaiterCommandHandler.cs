using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Waiters.Commands.UpdateWaiter;

internal sealed class UpdateWaiterCommandHandler : ICommandHandler<UpdateWaiterCommand>
{

    private readonly IWaiterRepository _waiterRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<UpdateWaiterCommandHandler> _logger;

    public UpdateWaiterCommandHandler(IWaiterRepository waiterRepository, IShiftRepository shiftRepository, ILogger<UpdateWaiterCommandHandler> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
        _shiftRepository = shiftRepository;
    }

    public async Task<ApiOperationResult> Handle(UpdateWaiterCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var shift = await _shiftRepository.SearchByIdAsync(command.ShiftId, cancellationToken);
            if (shift == null)
                return ApiOperationResult.Fail(WaiterError.RelatedShiftNotFound($"{command.FirstName} {command.LastName}"));

            var waiter = await _waiterRepository.SearchByIdAsync(command.Id,cancellationToken);

            if (waiter is null)
                return ApiOperationResult.Fail(WaiterError.NotFound(command.Id));

            waiter = command.Adapt<Waiter>();
            await _waiterRepository.UpdateAsync(waiter);

            return ApiOperationResult.Success(waiter);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception ocurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name, ex.Message, ApiErrorType.Failure));
        }
    }
}