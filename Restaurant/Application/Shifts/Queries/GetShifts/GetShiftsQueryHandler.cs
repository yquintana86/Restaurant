using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;
using System.Globalization;

namespace Application.Shifts.Queries.GetShiftByLookup;

internal class GetShiftsQueryHandler : IQueryHandler<GetShiftsQuery, List<SelectItem>>
{
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<GetShiftsQueryHandler> _logger;

    public GetShiftsQueryHandler(IShiftRepository shiftRepository, ILogger<GetShiftsQueryHandler> logger)
    {
        _shiftRepository = shiftRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<List<SelectItem>>> Handle(GetShiftsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var shifts = await _shiftRepository.GetAllShiftsAsync(cancellationToken);

            var items = shifts.Select(s => new SelectItem
            {
                Id = s.Id.ToString(CultureInfo.InvariantCulture),
                Text = s.Name,
            }).ToList();

            return ApiOperationResult.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error has ocurred:{}", ex.Message);
            return ApiOperationResult.Fail<List<SelectItem>> ("Error",ex.Message, ApiErrorType.Failure);
        }
    }
}
