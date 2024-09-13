using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Shifts.Commands.CreateShift;

internal sealed class CreateShiftCommandHandler : ICommandHandler<CreateShiftCommand>
{
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<CreateShiftCommandHandler> _logger;

    public CreateShiftCommandHandler(IShiftRepository shiftRepository, ILogger<CreateShiftCommandHandler> logger)
    {
        _shiftRepository = shiftRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(CreateShiftCommand shiftCommand, CancellationToken cancellationToken)
    {
        if (shiftCommand is null)
            return ApiOperationResult.Fail(ApiOperationError.NullReferenceError(typeof(CreateShiftCommand)));

        try
        {
            var shiftName = await _shiftRepository.SearchByNameAsync(shiftCommand.Name, cancellationToken);

            if (shiftName is not null)
                return ApiOperationResult.Fail(ShiftError.ShiftNameExist(shiftCommand.Name));

            var shift = new Shift
            (   shiftCommand.Name,
                shiftCommand.Begin,
                shiftCommand.End
            );

            await _shiftRepository.CreateAsync(shift, cancellationToken);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            var message = ex.Message;

            _logger.LogError(ex, "Exception ocurred: {Message}", message);

            return ApiOperationResult.Fail(new ApiOperationError("Create.Shift", ex.Message,ApiErrorType.Failure));
        }
    }
}
