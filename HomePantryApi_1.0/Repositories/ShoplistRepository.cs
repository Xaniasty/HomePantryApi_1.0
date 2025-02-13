﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;

public class ShoplistRepository : IShoplistRepository
{
    private readonly SpizDbContext _context;

    public ShoplistRepository(SpizDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Shoplist>> GetAllShopListsForUserAsync(int userId)
    {
        return await _context.Shoplists
            .Where(s => s.UserId == userId) 
            .ToListAsync();
    }

    public async Task<Shoplist> GetShopListByIdAsync(int shoplistId)
    {
        return await _context.Shoplists.FindAsync(shoplistId);
    }

    public async Task AddShopListsAsync(Shoplist shoplist)
    {
        await _context.Shoplists.AddAsync(shoplist);
        await _context.SaveChangesAsync(); 
    }

    public async Task UpdateShopListsAsync(Shoplist shoplist)
    {
        var existingShoplist = await _context.Shoplists.FindAsync(shoplist.Id);

        if (existingShoplist != null)
        {
            existingShoplist.ShoplistName = shoplist.ShoplistName;
            existingShoplist.Opis = shoplist.Opis;
            existingShoplist.DataAktualizacji = DateTime.Now;

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException($"Shoplist with Id {shoplist.Id} does not exist.");
        }
    }

    public async Task DeleteShopListAsync(int shoplistId)
    {
        var shoplist = await _context.Shoplists.FindAsync(shoplistId);
        if (shoplist != null)
        {
            _context.Shoplists.Remove(shoplist);
            await _context.SaveChangesAsync(); 
        }
    }

    public async Task DeleteShopListsForUserAsync(int userId)
    {
        var shoplists = await _context.Shoplists
            .Where(s => s.UserId == userId)
            .ToListAsync();

        if (shoplists.Any())
        {
            _context.Shoplists.RemoveRange(shoplists);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Shoplist> CreateShoplistFromGranaryAsync(int granaryId, int userId)
    {

        var productsInGranary = await _context.Productsingranaries
            .Where(p => p.GranaryId == granaryId)
            .ToListAsync();

        var newShoplist = new Shoplist
        {
            UserId = userId,
            ShoplistName = "Lista z magazynu", 
            DataUtworzenia = DateTime.Now, 
            DataAktualizacji = DateTime.Now, 
            Opis = "Lista stworzona z magazynu", 
            Productsinshoplists = productsInGranary.Select(p => new Productsinshoplist
            {
                ProductName = p.ProductName,
                Quantity = p.Quantity,
                IsLiquid = p.IsLiquid,
                Weight = p.Weight,
                Description = p.Description,
                InPackage = p.InPackage,
                DataZakupu = p.DataZakupu,
                DataWaznosci = p.DataWaznosci,
                Cena = p.Cena,
                Rodzaj = p.Rodzaj
            }).ToList()
        };

        await _context.Shoplists.AddAsync(newShoplist);
        await _context.SaveChangesAsync();

        return newShoplist;
    }


}
