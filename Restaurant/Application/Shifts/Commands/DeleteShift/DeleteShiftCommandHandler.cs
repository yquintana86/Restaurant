using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Shifts.Commands.DeleteShift;

internal sealed class DeleteShiftCommandHandler : ICommandHandler<DeleteShiftCommand>
{

    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<DeleteShiftCommandHandler> _logger;

    public DeleteShiftCommandHandler(IShiftRepository shiftRepository, ILogger<DeleteShiftCommandHandler> logger)
    {
        _shiftRepository = shiftRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteShiftCommand command, CancellationToken cancellationToken)
    {        
        try
        {            
            var shift = await _shiftRepository.SearchByIdAsync(command.Id, cancellationToken);

            if (shift is null)
                return ApiOperationResult.Fail(ShiftError.NotFound(command.Id));

            if(shift.Waiters.Any())
                return ApiOperationResult.Fail(ShiftError.ShiftHasWaiters());

            await _shiftRepository.DeleteAsync(command.Id);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception happened: {}", ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}

