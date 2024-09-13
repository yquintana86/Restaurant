using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Waiters.Commands.DeleteWaiter;

internal sealed class DeleteWaiterCommandHandler : ICommandHandler<DeleteWaiterCommand>
{
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<DeleteWaiterCommandHandler> _logger;
    public DeleteWaiterCommandHandler(IWaiterRepository waiterRepository, ILogger<DeleteWaiterCommandHandler> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteWaiterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var waiter = await _waiterRepository.SearchByIdAsync(request.Id, cancellationToken);
            if (waiter is null)
                return ApiOperationResult.Fail(WaiterError.NotFound(request.Id));

            if(waiter.Room != null)
                return ApiOperationResult.Fail(WaiterError.WaiterInChargeCannotBeDeleted());

            await _waiterRepository.DeleteAsync(request.Id);

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {}", ex.Message);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().Name, ex.Message,ApiErrorType.Failure));
        }
    }
}



