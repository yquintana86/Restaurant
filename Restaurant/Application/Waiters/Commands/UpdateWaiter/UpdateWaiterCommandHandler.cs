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
    private readonly ILogger<UpdateWaiterCommandHandler> _logger;

    public UpdateWaiterCommandHandler(IWaiterRepository waiterRepository, ILogger<UpdateWaiterCommandHandler> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(UpdateWaiterCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var waiter = await _waiterRepository.SearchByIdAsync(command.Id,cancellationToken);

            if (waiter is null)
                return ApiOperationResult.Fail(WaiterError.NotFound(command.Id));

            waiter.FirstName = command.FirstName;
            waiter.LastName = command.LastName;
            waiter.Salary = command.Salary;
            waiter.Start = command.Start;

            await _waiterRepository.UpdateAsync(waiter);

            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception ocurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name, ex.Message, ApiErrorType.Failure));
        }
    }
}