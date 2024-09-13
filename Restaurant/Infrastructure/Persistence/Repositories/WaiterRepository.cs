using Infrastructure.Extension;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedLib.Models.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Waiters.Queries.GetWaitersByFilter;
using Dapper;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence.Repositories;

public sealed class WaiterRepository : IWaiterRepository
{
    private readonly IEFCoreDbContext _eFCoreDbContext;
    private readonly IDapperDbContext _dapperDbContext;
    private readonly IMemoryCache _memoryCache;

    public WaiterRepository(IEFCoreDbContext restaurantDbContext, IDapperDbContext dapperDbContext, IMemoryCache memoryCache)
    {
        _eFCoreDbContext = restaurantDbContext;
        _dapperDbContext = dapperDbContext;
        _memoryCache = memoryCache;
    }

    public async Task<PagedResult<Waiter>> SearchByFilterAsync(GetWaitersByFilterQuery waiterFilter, CancellationToken cancellationToken = default)
    {
        if (waiterFilter == null)
            ArgumentNullException.ThrowIfNull(nameof(waiterFilter));

        var query = from w in _eFCoreDbContext.Waiters.Include(w => w.Shift).AsNoTracking()
                    select w;

        var firstName = waiterFilter!.FirstName;
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(w => w.FirstName != null && w.FirstName.StartsWith(firstName));
        }

        var lastName = waiterFilter.LastName;
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(w => w.LastName != null && w.LastName.StartsWith(lastName));
        }

        if (waiterFilter.SalaryFrom.HasValue)
        {
            query = query.Where(w => w.Salary >= waiterFilter.SalaryFrom);
        }

        if (waiterFilter.SalaryTo.HasValue)
        {
            query = query.Where(w => w.Salary <= waiterFilter.SalaryTo);
        }

        if (waiterFilter.ShiftId.HasValue)
        {
            query = query.Where(w => w.ShiftId == waiterFilter.ShiftId);
        }

        var result = await query.ToQuickPagedList(w => w.Id, waiterFilter.Page, waiterFilter.PageSize, waiterFilter.RequestCount, cancellationToken);

        return result;
    }

    public async Task<Waiter?> SearchByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            ArgumentOutOfRangeException.ThrowIfLessThan(id, 0);

        string key = $"GetWaiterById-{id}";

        var waiter = await _memoryCache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            return await _eFCoreDbContext.Waiters
                                            .Include(w => w.Shift)
                                            .Include(w => w.Room)
                                            .AsNoTracking()
                                            .AsSplitQuery()
                                            .FirstOrDefaultAsync(w => w.Id == id);

        });

        return waiter;
    }

    public async Task<Waiter?> SearchByCharAsync(string characters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(characters))
            ArgumentNullException.ThrowIfNull(nameof(characters));

        var waiterList = await _eFCoreDbContext.Waiters
                                                .FromSqlRaw($"EXECUTE WaitersByName @Characters = @char", new SqlParameter("char", characters)).ToListAsync(cancellationToken);

        var waiter = waiterList.FirstOrDefault();

        return waiter;
    }

    public async Task<int> CreateAsync(Waiter waiter)
    {
        _eFCoreDbContext.Waiters.Add(waiter!);
        await _eFCoreDbContext.SaveChangesAsync();

        return waiter!.Id;
    }

    public async Task UpdateAsync(Waiter waiter)
    {
        var id = waiter.Id;
        var waiterResponse = await _eFCoreDbContext.Waiters.FirstOrDefaultAsync(w => w.Id == id);

        waiterResponse!.FirstName = waiter.FirstName;
        waiterResponse.LastName = waiter.LastName;
        waiterResponse.Salary = waiter.Salary;
        waiterResponse.ShiftId = waiter.ShiftId;

        _eFCoreDbContext.Waiters.Update(waiterResponse);    //SetValues doesn't update navigation properties, if you try it will give you an error
                                                            //Then we need to take care when the Reservations and ShiftId will be updated. 
        await _eFCoreDbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var waiter = await _eFCoreDbContext.Waiters.FirstOrDefaultAsync(w => w.Id == id);

        _eFCoreDbContext.Waiters.Remove(waiter!);
        await _eFCoreDbContext.SaveChangesAsync();

        return waiter!.Id;
    }

    public async Task<IList<Waiter>> GetAllWaitersAsync(bool IsRoomResponsable)
    {
        using var connection = _dapperDbContext.Connection();

        var sql = IsRoomResponsable ? """SELECT w.Id, w.FirstName, w.LastName From Waiters w INNER JOIN Rooms r ON w.Id = r.WaiterId Order By w.Id """
                                    : """SELECT Id, FirstName, LastName From Waiters""";
        
        var result = await connection.QueryAsync<Waiter>(sql);

        return result.ToList();
    }
}
