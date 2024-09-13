using Application.Waiters;
using Application.Waiters.Queries.GetWaitersByFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories;

public interface IWaiterRepository
{
    Task<IList<Waiter>> GetAllWaitersAsync(bool IsRoomResponsable);
    Task<PagedResult<Waiter>> SearchByFilterAsync(GetWaitersByFilterQuery waiterFilter, CancellationToken cancellationToken = default);
    Task<Waiter?> SearchByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Waiter waiter);
    Task UpdateAsync(Waiter waiter);
    Task<int> DeleteAsync(int id);
    Task<Waiter?> SearchByCharAsync(string characters, CancellationToken cancellationToken = default);

}
