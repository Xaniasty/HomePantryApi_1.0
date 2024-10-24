using HomePantryApi_1._0.Models;

namespace HomePantryApi_1._0.Repositories.Interfaces
{
    public interface IProductsInGranaryRepository
    {
        Task<IEnumerable<Productsingranary>> GetProductsByGranaryIdAsync(int granaryId);
        Task<Productsingranary> GetProductByIdAsync(int productId);
        Task AddProductToGranaryAsync(Productsingranary product);
        Task UpdateProductInGranaryAsync(Productsingranary product);
        Task DeleteProductFromGranaryAsync(int productId);
        Task DeleteAllProductsFromGranaryAsync(int granaryId);
    }
}
