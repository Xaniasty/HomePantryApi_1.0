using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace HomePantryApi_1._0.Repositories;

public class ProductsInGranaryRepository : IProductsInGranaryRepository
{
    private readonly SpizDbContext _context;

    public ProductsInGranaryRepository(SpizDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Productsingranary>> GetProductsByGranaryIdAsync(int granaryId)
    {
        return await _context.Productsingranaries.Where(p => p.GranaryId == granaryId).ToListAsync();
    }

    public async Task<Productsingranary> GetProductByIdAsync(int productId)
    {
        return await _context.Productsingranaries.FindAsync(productId);
    }

    public async Task AddProductToGranaryAsync(Productsingranary product)
    {

        var granaryExists = await _context.Granaries.AnyAsync(g => g.Id == product.GranaryId);
        if (!granaryExists)
        {
            throw new Exception($"Granary nie istnieje.");
        }

        await _context.Productsingranaries.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductInGranaryAsync(Productsingranary product)
    {
       
        var existingEntity = await _context.Productsingranaries.FindAsync(product.ProductId);

        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Detached;
        }

        _context.Update(product);
        await _context.SaveChangesAsync();

    }

    public async Task DeleteProductFromGranaryAsync(int productId)
    {
        var product = await GetProductByIdAsync(productId);
        if (product != null)
        {
            _context.Productsingranaries.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllProductsFromGranaryAsync(int granaryId)
    {
        var products = await GetProductsByGranaryIdAsync(granaryId);
        _context.Productsingranaries.RemoveRange(products);
        await _context.SaveChangesAsync();
    }
}
