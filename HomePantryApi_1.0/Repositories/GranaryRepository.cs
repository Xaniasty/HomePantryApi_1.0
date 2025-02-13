﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomePantryApi_1._0.Repositories.Interfaces;
using HomePantryApi_1._0.Models;
using System;

public class GranaryRepository : IGranaryRepository
{
    private readonly SpizDbContext _context;

    public GranaryRepository(SpizDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Granary>> GetAllGranariesForUserAsync(int userId)
    {
        return await _context.Granaries.Where(g => g.UserId == userId).ToListAsync();
    }

    public async Task<Granary?> GetGranaryByIdAsync(int granaryId)
    {
        return await _context.Granaries
            .Include(g => g.Productsingranaries)
            .FirstOrDefaultAsync(g => g.Id == granaryId);
    }

    public async Task AddGranaryAsync(Granary granary)
    {
        await _context.Granaries.AddAsync(granary);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGranaryAsync(Granary granary)
    {
        var existingGranary = await _context.Granaries.FindAsync(granary.Id);

        if (existingGranary != null)
        {
            existingGranary.GranaryName = granary.GranaryName;
            existingGranary.Opis = granary.Opis;
            existingGranary.DataAktualizacji = DateTime.Now; 

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException($"Granary with Id {granary.Id} does not exist.");
        }
    }

    public async Task DeleteGranaryAsync(int granaryId)
    {
        var granary = await _context.Granaries.Include(g => g.Productsingranaries)
                                              .FirstOrDefaultAsync(g => g.Id == granaryId);

        if (granary == null)
        {
            throw new ArgumentException("Granary not found.");
        }

        
        _context.Productsingranaries.RemoveRange(granary.Productsingranaries);

        _context.Granaries.Remove(granary);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllGranariesForUserAsync(int userId)
    {
        var granaries = await _context.Granaries.Include(g => g.Productsingranaries)
                                                .Where(g => g.UserId == userId)
                                                .ToListAsync();

        foreach (var granary in granaries)
        {
            _context.Productsingranaries.RemoveRange(granary.Productsingranaries);
        }

        _context.Granaries.RemoveRange(granaries);
        await _context.SaveChangesAsync();
    }

    public async Task<Granary> CreateGranaryFromShoplistAsync(int shoplistId, int userId)
    {
        var productsInShoplist = await _context.Productsinshoplists
            .Where(p => p.ShoplistId == shoplistId)
            .ToListAsync();

        var newGranary = new Granary
        {
            UserId = userId,
            GranaryName = "Magazyn z listy", 
            DataUtworzenia = DateTime.Now,
            DataAktualizacji = DateTime.Now,
            Opis = "Magazyn stworzony z listy zakupów.",

            Productsingranaries = productsInShoplist.Select(p => new Productsingranary
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



        await _context.Granaries.AddAsync(newGranary);
        await _context.SaveChangesAsync();

        return newGranary;
    }


}