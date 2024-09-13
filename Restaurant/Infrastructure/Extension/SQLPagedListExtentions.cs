using Microsoft.EntityFrameworkCore;
using SharedLib.Models.Common;
using System.Linq.Expressions;
using static Dapper.SqlMapper;


namespace Infrastructure.Extension;

public static class SQLPagedListExtentions
{
    public static async Task<PagedResult<T>> ToQuickPagedList<T,TKey>(this IQueryable<T> query,Expression<Func<T,TKey>> orderBy, int currentPage, int pageSize, bool? requestPaging, CancellationToken cancellationToken = default) where T : class
    {
        bool hasNext = false;
        int count = 0;
        int? totalItemCount = null; 
        PagedResult<T> paged = new PagedResult<T>();

        if (requestPaging.HasValue && requestPaging.Value == true)
        {
            totalItemCount = await query.CountAsync(cancellationToken); 
        }

        var result = await query.OrderBy(orderBy).Skip((currentPage - 1) * pageSize).Take(pageSize + 1).ToListAsync(cancellationToken);

        count = result.Count;
        if (count != 0)
        {
            if (count == pageSize + 1)
            {
                hasNext = true;
                result.RemoveAt(pageSize);
            }

            paged = new PagedResult<T>
            {
                Currentpage = currentPage,
                PageSize = pageSize,
                HasNextPage = hasNext,
                ItemCount = !hasNext ? count : count - 1,
                TotalItemCount = totalItemCount,
                PageCount = totalItemCount.HasValue ? (int)Math.Ceiling((double)totalItemCount / pageSize) : totalItemCount,
                Results = result?.ToList()
            };
        }
        return paged;
    }
}
