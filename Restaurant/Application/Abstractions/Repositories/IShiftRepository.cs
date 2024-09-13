using Application.Shifts.Queries.GetShiftsByFilter;
using Domain.Entities;
using SharedLib.Models.Common;

namespace Application.Abstractions.Repositories;

public interface IShiftRepository
{
    Task<IList<Shift>> GetAllShiftsAsync(CancellationToken cancellationToken = default);
    Task<Shift?> SearchByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Shift?> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<PagedResult<Shift>> SearchByFilterAsync(GetShiftsByFilterQuery filter, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Shift shift, CancellationToken cancellationToken);
    Task UpdateAsync(Shift shift);
    Task<int> DeleteAsync(int id);    
}
