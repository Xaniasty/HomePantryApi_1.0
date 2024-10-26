using HomePantryApi_1._0.Models;

namespace HomePantryApi_1._0.Repositories.Interfaces;

public interface IShoplistRepository
{
    Task<IEnumerable<Shoplist>> GetAllShopListsForUserAsync(int userId);
    Task<Shoplist> GetShopListByIdAsync(int shoplistId);
    Task AddShopListsAsync(Shoplist shoplist);
    Task UpdateShopListsAsync(Shoplist shoplist);
    Task DeleteShopListAsync(int shoplistId);
    Task DeleteShopListsForUserAsync(int userId);
    Task<Shoplist> CreateShoplistFromGranaryAsync(int granaryId, int userId);


}
