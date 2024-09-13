using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Shifts.Commands.UpdateShift;

internal sealed class UpdateShiftCommandHandler : ICommandHandler<UpdateShiftCommand>
{
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<UpdateShiftCommandHandler> _logger;

    public UpdateShiftCommandHandler(IShiftRepository shiftRepository, ILogger<UpdateShiftCommandHandler> logger)
    {
        _shiftRepository = shiftRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(UpdateShiftCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return ApiOperationResult.Fail(ApiOperationError.NullReferenceError(typeof(UpdateShiftCommand)));

        try
        {
            var shift = await _shiftRepository.SearchByIdAsync(command.Id, cancellationToken);

            if (shift is null)
            {
                return ApiOperationResult.Fail(ShiftError.NotFound(command.Id));
            }

            shift = new Shift(command.Id, command.Name, command.Begin, command.End);

            await _shiftRepository.UpdateAsync(shift);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception has occurred: {}", ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}
