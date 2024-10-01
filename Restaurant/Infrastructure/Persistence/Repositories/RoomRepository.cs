using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Rooms.Queries.GetRoomsByFilter;
using Dapper;
using Domain.Entities;
using Infrastructure.Extension;
using Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharedLib.Models.Common;

namespace Infrastructure.Persistence.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly EFCoreDbContext _eFCoreDbContext;
    private readonly IDapperDbContext _dapperDbContext;
    private readonly IMemoryCache _memoryCache;

    public RoomRepository(EFCoreDbContext eFCoreDbContext, IDapperDbContext dapperDbContext, IMemoryCache memoryCache)
    {
        _eFCoreDbContext = eFCoreDbContext;
        _dapperDbContext = dapperDbContext;
        _memoryCache = memoryCache;
    }

    public async Task<int> CreateAsync(Room room, CancellationToken cancellationToken = default)
    {
        _eFCoreDbContext.Add(room);
        await _eFCoreDbContext.SaveChangesAsync(cancellationToken);
        return room.Id;
    }

    public async Task<IEnumerable<Room>> GetRoomsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _dapperDbContext.Connection();

        var sql = """"Select Id, Name, Theme, Description, WaiterId From Rooms"""";

        var result = await connection.QueryAsync<Room>(sql);
        
        return result;
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var room = await SearchByIdAsync(id);

        if (room is null)
            ArgumentNullException.ThrowIfNull(nameof(room));

        _eFCoreDbContext.Remove(room!);
        await _eFCoreDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id == 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        using var connection = _dapperDbContext.Connection();

        var sql = "SELECT r.Id FROM Rooms r WHERE r.Id = @id";

        int result = await connection.QuerySingleOrDefaultAsync<int>(sql, new { id });

        return result > 0;
    }

    public async Task<bool> ExistNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            ArgumentNullException.ThrowIfNull(nameof(name));

        using var connection = _dapperDbContext.Connection();

        var sql = "SELECT r.Id FROM Rooms r WHERE r.Name like @name";

        int id = await connection.QuerySingleOrDefaultAsync<int>(sql, new { name });

        return id > 0;
    }

    public Task<PagedResult<Room>> GetbyFilterAsync(GetRoomsByFilterQuery filter, CancellationToken cancellationToken = default)
    {
        IQueryable<Room> query = from r in _eFCoreDbContext.Rooms.Include(r => r.Waiter).AsNoTracking()
                                 select r;

        var name = filter.Name;
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(r => r.Name.StartsWith(name!));
        }

        if (filter.WaiterId > 0)
        {
            query = query.Where(r => r.Waiter.Id == filter.WaiterId.Value);
        }

        var paged = query.ToQuickPagedList(r => r.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);

        return paged;
    }

    public async Task<Room?> SearchByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        string key = $"RoomById-{id}";

        Room? room = await _memoryCache.GetOrCreateAsync(key, async (entry) =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            return await _eFCoreDbContext.Rooms
                                            .Include(r => r.Waiter)
                                            .Include(r => r.Tables)
                                            .AsSplitQuery()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(r => r.Id == id);
        });

        return room;
    }

    public async Task<Room?> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            ArgumentNullException.ThrowIfNull(nameof(name));

        string key = $"RoomByName-{name}";

        Room? room = await _memoryCache.GetOrCreateAsync(key, async (entry) =>
        {

            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            return await _eFCoreDbContext.Rooms
                                            .Include(r => r.Waiter)
                                            .FirstOrDefaultAsync(e => e.Name == name);
        });

        return room;
    }

    public async Task UpdateAsync(Room room)
    {
        var roomDb = await _eFCoreDbContext.Rooms.SingleOrDefaultAsync(r => r.Id == room.Id);

        if (roomDb is not null)
        {
            roomDb.WaiterId = room.WaiterId;
            roomDb.Theme = room.Theme;
            roomDb.Description = room.Description;

            _eFCoreDbContext.Update(roomDb);
            await _eFCoreDbContext.SaveChangesAsync();
        }
    }
}
