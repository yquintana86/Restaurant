using Application.RoomTable.Queries.GetTablesbyFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories
{
    public interface IRoomTableRepository
    {
        Task<int> CreateAsync(Domain.Entities.RoomTable table);
        Task<Domain.Entities.RoomTable?> SearchByIdAsync(int tableId, int roomId, CancellationToken cancellationToken = default);
        Task<PagedResult<Domain.Entities.RoomTable>> SearchByFilterAsync(GetRoomTablesByFilterQuery filter, CancellationToken cancellationToken = default);
        Task UpdateAsync(Domain.Entities.RoomTable table);
        Task<int> DeleteAsync(int id, int roomId);
    }
}
