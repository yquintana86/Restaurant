using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.RoomTable.Queries.GetTablesbyFilter;
using Dapper;
using Domain.Entities;
using Infrastructure.Extension;
using Microsoft.EntityFrameworkCore;
using SharedLib.Models.Common;


namespace Infrastructure.Persistence.Repositories;

public class RoomTableRepository : IRoomTableRepository
{
    private readonly IEFCoreDbContext _eFCoreDbContext;
    private readonly IDapperDbContext _dapperDbContext;


    public RoomTableRepository(IEFCoreDbContext eFCoreDbContext,  IDapperDbContext dapperDbContext)
    {
        _eFCoreDbContext = eFCoreDbContext;
        _dapperDbContext = dapperDbContext;
    }

    public async Task<int> CreateAsync(RoomTable table)
    {
        ThrowIfBadIds(null, table.RoomId);

        _eFCoreDbContext.RoomTables.Add(table);
        await _eFCoreDbContext.SaveChangesAsync();

        return table.Id;
    }

    public async Task<PagedResult<RoomTable>> SearchByFilterAsync(GetRoomTablesByFilterQuery filter, CancellationToken cancellationToken = default)
    {
        if (filter is null)
            ArgumentNullException.ThrowIfNull(nameof(filter));

        var query = from rt in _eFCoreDbContext.RoomTables
                                                .Include(t => t.Waiter)
                                                .Include(t => t.Room)
                                                .AsNoTracking()
                                                .AsSplitQuery()
                    select rt;

        var a  = _eFCoreDbContext.RoomTables.ToList();

        var status = filter!.Status;
        if (status.HasValue)
        {
            query = query.Where(tbl => tbl.Status == status);
        }

        var totalQtyFrom = filter.TotalQtyFrom;
        if (totalQtyFrom.HasValue)
        {
            query = query.Where(tbl => tbl.TotalQty >= totalQtyFrom);
        }

        var totalQtyTo = filter.TotalQtyTo;
        if (totalQtyTo.HasValue)
        {
            query = query.Where(tbl => tbl.TotalQty <= totalQtyTo);
        }

        var waiterId = filter.WaiterId;
        if (waiterId.HasValue)
        {
            query = query.Where(tbl => tbl.WaiterId == waiterId);
        }

        var roomId = filter.RoomId;
        if (roomId.HasValue)
        {
            query = query.Where(tbl => tbl.RoomId == roomId);
        }

        var result = await query.ToQuickPagedList(tbl => tbl.Id, filter.Page, filter.PageSize, filter.RequestCount, cancellationToken);

        return result;

    }

    public async Task<RoomTable?> SearchByIdAsync(int tableId, int roomId, CancellationToken cancellationToken = default)
    {
        ThrowIfBadIds(tableId, roomId);

        var dbTable = await _eFCoreDbContext.RoomTables
                                            .AsNoTracking().
                                                FirstOrDefaultAsync(tbl => tbl.Id == tableId && tbl.RoomId == roomId, cancellationToken);

        return dbTable;
    }

    public async Task UpdateAsync(RoomTable table)
    {
        ThrowIfBadIds(table.Id, table.RoomId);

        var dbTable = await _eFCoreDbContext.RoomTables.FirstOrDefaultAsync(tbl => tbl.Id == table.Id && tbl.RoomId == table.RoomId);

        if (dbTable is null)
            ArgumentNullException.ThrowIfNull($"RoomTable with id:{table.Id} and roomId:{table.RoomId} not found");

        dbTable!.RoomId = table.RoomId;
        dbTable.Status = table.Status;
        dbTable.TotalQty = table.TotalQty;
        dbTable.WaiterId = table.WaiterId;

        _eFCoreDbContext.RoomTables.Update(dbTable);
        await _eFCoreDbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id, int roomId)
    {
        ThrowIfBadIds(id, roomId);

        var dbTable = await _eFCoreDbContext.RoomTables.FirstOrDefaultAsync(tbl => tbl.Id == id && tbl.RoomId == roomId);

        if (dbTable is null)
            ArgumentNullException.ThrowIfNull($"RoomTable with id:{id} and roomId:{roomId} not found");

        _eFCoreDbContext.RoomTables.Remove(dbTable!);
        await _eFCoreDbContext.SaveChangesAsync();

        return id;
    }

    private static void ThrowIfBadIds(int? id, int roomId)
    {
        if (id.HasValue && id.Value == 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id.Value, 0);

        if (roomId == 0)
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(roomId, 0);
    }
}
