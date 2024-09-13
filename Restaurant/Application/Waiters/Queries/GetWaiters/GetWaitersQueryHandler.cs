using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using SharedLib.Models.Common;
using System.Globalization;

namespace Application.Waiters.Queries.GetWaiters;

internal sealed class GetWaitersQueryHandler : IQueryHandler<GetWaitersQuery, List<SelectItem>>
{
    private readonly IWaiterRepository _waiterRepository;
    private readonly ILogger<GetWaitersQueryHandler> _logger;

    public GetWaitersQueryHandler(IWaiterRepository waiterRepository, ILogger<GetWaitersQueryHandler> logger)
    {
        _waiterRepository = waiterRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<List<SelectItem>>> Handle(GetWaitersQuery request, CancellationToken cancellationToken)
    {

        try
        {
            var result = await _waiterRepository.GetAllWaitersAsync(request.IsRoomResponsable);

            var selectItems = result.Select(w => new SelectItem
            {
                Id = w.Id.ToString(CultureInfo.CurrentCulture),
                Text = w.FirstName + " " + w.LastName
            }).ToList();

            return ApiOperationResult.Success(selectItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ocurred: {}", ex.Message);
            return ApiOperationResult.Fail<List<SelectItem>>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }

    }
}
