using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Waiters.Commands.CreateWaiter;

internal sealed class CreateWaiterCommandHandler : ICommandHandler<CreateWaiterCommand>
{
    private readonly IWaiterRepository _waiterRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<CreateWaiterCommandHandler> _logger;

    public CreateWaiterCommandHandler(IWaiterRepository waiterRepository, ILogger<CreateWaiterCommandHandler> logger, IShiftRepository shiftRepository)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
        _shiftRepository = shiftRepository;
    }

    public async Task<ApiOperationResult> Handle(CreateWaiterCommand command, CancellationToken cancellationToken = default)
    {

        if (command is null)
            return ApiOperationResult.Fail(ApiOperationError.NullReferenceError(typeof(CreateWaiterCommand)));

        try
        {
            var shift = await _shiftRepository.SearchByIdAsync(command.ShiftId, cancellationToken);
            if (shift is null)
                return ApiOperationResult.Fail(WaiterError.RelatedShiftNotFound($"{command.FirstName} {command.LastName}"));

            var waiter = new Waiter
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Salary = command.Salary,
                ShiftId = command.ShiftId
            };  
            
            await _waiterRepository.CreateAsync(waiter);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name, ex.Message,ApiErrorType.Failure));
        }
    }
}

