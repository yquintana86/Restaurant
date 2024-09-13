using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Shifts.Queries.GetShiftsByFilter;
using Dapper;
using Domain.Entities;
using Infrastructure.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLib.Models.Common;

namespace Infrastructure.Persistence.Repositories;

public class ShiftRepository : IShiftRepository
{
    private readonly IEFCoreDbContext _eFCoreDbContext;
    private readonly IDapperDbContext _dapperDbContext;


    public ShiftRepository(IEFCoreDbContext eFCoreDbContext,  IDapperDbContext dapperDbContext)
    {
        _eFCoreDbContext = eFCoreDbContext;
        _dapperDbContext = dapperDbContext;
    }

    public async Task<int> CreateAsync(Shift shift, CancellationToken cancellationToken)
    {
        _eFCoreDbContext.Shifts.Add(shift);
        await _eFCoreDbContext.SaveChangesAsync(cancellationToken);

        return shift.Id;
    }

    public async Task<PagedResult<Shift>> SearchByFilterAsync(GetShiftsByFilterQuery filter, CancellationToken cancellationToken = default)
    {
        if (filter is null)
            ArgumentNullException.ThrowIfNull(nameof(filter));

        var query = from f in _eFCoreDbContext.Shifts.AsNoTracking()
                    select f;

        var filterName = filter?.Name;
        if (!string.IsNullOrWhiteSpace(filterName))
        {
            query = query.Where(sh => sh.Name != null && sh.Name.StartsWith(filterName));
        }

        var startTime = filter!.Begin;
        if (startTime.HasValue)
        {
            query = query.Where(fm => startTime.Value.AddDays(-1) <= fm.StartTime);
        }

        var endTime = filter.End;
        if (endTime.HasValue)
        {
            query = query.Where(fm => endTime.Value.AddDays(1) >= fm.EndTime);
        }

        var result = await query.ToQuickPagedList(f => f.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);

        return result;

    }

    public async Task<Shift?> SearchByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var shift = await _eFCoreDbContext.Shifts
                                            .Include(sh => sh.Waiters)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(sh => sh.Id == id, cancellationToken); 

        return shift;
    }

    public async Task<Shift?> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(name));

        using var connection = _dapperDbContext.Connection();

        //For security reasons it is better to create a store procedure and execute it than declare a raw sql query in a dll. 
        var sql = """"SELECT Id, Name, StartTime, EndTime FROM Shifts WHERE Name like @name"""";

        var filter = await connection.QueryFirstOrDefaultAsync<Shift>(sql, new { name });

        return filter;
    }  

    public async Task UpdateAsync(Shift shift)
    {
        var id = shift.Id;
        if (id <= 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var shiftResult = await _eFCoreDbContext.Shifts.FirstOrDefaultAsync(sh => sh.Id == id);

        if (shiftResult == null)
            ArgumentNullException.ThrowIfNull($"ShiftRepository not found | ID:{id}");

        _eFCoreDbContext.Shifts.Entry(shiftResult!).CurrentValues.SetValues(shift!);    //SetValues doesn't update navigation properties, if you try it will give you an error

        await _eFCoreDbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> DeleteAsync(int id)
    {
        if (id <= 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var shift = await _eFCoreDbContext.Shifts.FirstOrDefaultAsync(sh => sh.Id == id).ConfigureAwait(false);

        if (shift == null)
            ArgumentNullException.ThrowIfNull($"ShiftRepository not found | ID:{id}");

        _eFCoreDbContext.Shifts.Remove(shift!);
        await _eFCoreDbContext.SaveChangesAsync().ConfigureAwait(false);

        return id;
    }

    public async Task<IList<Shift>> GetAllShiftsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _dapperDbContext.Connection();

        var sql = """SELECT Id, Name From Shifts""";

        var shifts = await connection.QueryAsync<Shift>(sql);

        return shifts?.ToList() ?? new List<Shift>();
    }
}
