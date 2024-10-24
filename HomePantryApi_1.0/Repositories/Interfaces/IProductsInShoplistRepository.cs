using HomePantryApi_1._0.Models;

namespace HomePantryApi_1._0.Repositories.Interfaces;

public interface IProductsInShoplistRepository
{
    Task<IEnumerable<Productsinshoplist>> GetProductsByShoplistIdAsync(int shoplistId);
    Task<Productsinshoplist> GetProductByIdAsync(int productId);
    Task AddProductToShoplistAsync(Productsinshoplist product);
    Task UpdateProductInShoplistAsync(Productsinshoplist product);
    Task DeleteProductFromShoplistAsync(int productId);
    Task DeleteAllProductsFromShoplistAsync(int shoplistId);
}