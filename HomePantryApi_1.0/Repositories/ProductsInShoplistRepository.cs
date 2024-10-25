using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace HomePantryApi_1._0.Repositories;

public class ProductsInShoplistRepository : IProductsInShoplistRepository
{
    private readonly SpizDbContext _context;

    public ProductsInShoplistRepository(SpizDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Productsinshoplist>> GetProductsByShoplistIdAsync(int shoplistId)
    {
        return await _context.Productsinshoplists.Where(p => p.ShoplistId == shoplistId).ToListAsync();
    }

    public async Task<Productsinshoplist> GetProductByIdAsync(int productId)
    {
        return await _context.Productsinshoplists.FindAsync(productId);
    }

    public async Task AddProductToShoplistAsync(Productsinshoplist product)
    {

        var shoplistExists = await _context.Shoplists.AnyAsync(s => s.Id == product.ShoplistId);
        if (!shoplistExists)
        {
            throw new Exception($"Shoplist nie istnieje.");
        }

        await _context.Productsinshoplists.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductInShoplistAsync(Productsinshoplist product)
    {
        var existingEntity = await _context.Productsinshoplists.FindAsync(product.ProductId);

        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }

        _context.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductFromShoplistAsync(int productId)
    {
        var product = await GetProductByIdAsync(productId);
        if (product != null)
        {
            _context.Productsinshoplists.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllProductsFromShoplistAsync(int shoplistId)
    {
        var products = await GetProductsByShoplistIdAsync(shoplistId);
        _context.Productsinshoplists.RemoveRange(products);
        await _context.SaveChangesAsync();
    }
}
