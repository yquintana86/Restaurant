using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Mapster;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Shifts.Queries.GetShiftById;

internal sealed class GetShiftByIdQueryHandler : IQueryHandler<GetShiftByIdQuery, ShiftResponse>
{
    private readonly IShiftRepository _shiftRepository;
    private readonly ILogger<GetShiftByIdQueryHandler> _logger;

    public GetShiftByIdQueryHandler(IShiftRepository shiftRepository, ILogger<GetShiftByIdQueryHandler> logger)
    {
        _shiftRepository = shiftRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<ShiftResponse>> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _shiftRepository.SearchByIdAsync(request.ShiftId, cancellationToken);

            if(result is null)
                return ApiOperationResult.Fail<ShiftResponse>(ShiftError.NotFound(request.ShiftId));
            
            var response = result.Adapt<ShiftResponse>();

            return ApiOperationResult.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has occurred: {}", ex.Message);

            return ApiOperationResult.Fail<ShiftResponse>(ex.GetType().Name, ex.Message,ApiErrorType.Failure);
        }

        
    }
}
