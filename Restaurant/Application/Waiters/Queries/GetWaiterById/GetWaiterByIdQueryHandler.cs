using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Waiters.Queries.GetWaiterById;

internal sealed class GetWaiterByIdQueryHandler : IQueryHandler<GetWaiterByIdQuery, WaiterResponse>
{
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<GetWaiterByIdQueryHandler> _logger;
    public GetWaiterByIdQueryHandler(IWaiterRepository waiterRepository, ILogger<GetWaiterByIdQueryHandler> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<WaiterResponse>> Handle(GetWaiterByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if(request.Id <= 0)
                return ApiOperationResult.Fail<WaiterResponse>(WaiterError.WaiterIdInvalid());

            var waiter = await _waiterRepository.SearchByIdAsync(request.Id, cancellationToken);
                
            if (waiter is null)
                return ApiOperationResult.Fail<WaiterResponse>(WaiterError.NotFound(request.Id));

            var waiterResponse = new WaiterResponse( waiter.Id, waiter.FirstName, waiter.LastName,
                                                     waiter.Salary, waiter.Start, waiter.End, waiter.Room?.Name);

            return ApiOperationResult.Success(waiterResponse);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has ocurred: {}", ex.Message);
            return ApiOperationResult.Fail<WaiterResponse>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}
