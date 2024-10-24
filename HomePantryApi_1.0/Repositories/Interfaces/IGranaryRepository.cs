using HomePantryApi_1._0.Models;

namespace HomePantryApi_1._0.Repositories.Interfaces;

public interface IGranaryRepository
{
    Task<IEnumerable<Granary>> GetAllGranariesForUserAsync(int userId);
    Task<Granary> GetGranaryByIdAsync(int granaryId);
    Task AddGranaryAsync(Granary granary);
    Task UpdateGranaryAsync(Granary granary);
    Task DeleteGranaryAsync(int granaryId);
    Task DeleteAllGranariesForUserAsync(int userId);

}
