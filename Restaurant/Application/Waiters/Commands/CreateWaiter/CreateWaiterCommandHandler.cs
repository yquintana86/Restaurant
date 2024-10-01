using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Waiters.Commands.CreateWaiter;

internal sealed class CreateWaiterCommandHandler : ICommandHandler<CreateWaiterCommand>
{
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<CreateWaiterCommandHandler> _logger;

    public CreateWaiterCommandHandler(IWaiterRepository waiterRepository, ILogger<CreateWaiterCommandHandler> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(CreateWaiterCommand command, CancellationToken cancellationToken = default)
    {

        if (command is null)
            return ApiOperationResult.Fail(ApiOperationError.NullReferenceError(typeof(CreateWaiterCommand)));

        try
        {
            var waiter = new Waiter
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Salary = command.Salary,
                Start = DateTime.Now
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

