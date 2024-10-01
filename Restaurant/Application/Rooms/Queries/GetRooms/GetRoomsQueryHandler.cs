using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;
using System.Globalization;

namespace Application.Rooms.Queries.GetRooms;

internal class GetRoomsQueryHandler : IQueryHandler<GetRoomsQuery, List<SelectItem>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<GetRoomsQueryHandler> _logger;

    public GetRoomsQueryHandler(IRoomRepository roomRepository, ILogger<GetRoomsQueryHandler> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<List<SelectItem>>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rooms = await _roomRepository.GetRoomsAsync(cancellationToken);

            var result = rooms.Select(r => new SelectItem
            {
                Id = r.Id.ToString(CultureInfo.CurrentCulture),
                Text = r.Name
            }).ToList();

            return ApiOperationResult.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error has occurred: {}", ex.Message);
            return ApiOperationResult.Fail<List<SelectItem>>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}
