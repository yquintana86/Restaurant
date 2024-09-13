using Application.Rooms.Queries.GetRoomsByFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories;

public interface IRoomRepository
{
    Task<Room?> SearchByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Room?> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Room>> GetbyFilterAsync(GetRoomsByFilterQuery filter, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Room room, CancellationToken cancellationToken = default);
    Task UpdateAsync(Room room);
    Task DeleteAsync(int id);

}
