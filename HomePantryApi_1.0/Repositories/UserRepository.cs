namespace HomePantryApi_1._0.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



public class UserRepository : IUserRepository
{
    private readonly SpizDbContext _context;

    public UserRepository(SpizDbContext context)
    {
        _context = context;
    }

    private async Task<bool> UserExistsAsync(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Granaries)
            .Include(u => u.Shoplists)
            .ToListAsync();
    }

    public async Task<User> GetUsersByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            return user;
        }
        else
        {
            throw new Exception("User not found.");
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        var existingUserWithLogin = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

        if (existingUserWithEmail != null || existingUserWithLogin != null) {
            throw new Exception("Email or Login is already taken");
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(); 
        return user;
    }

    public async Task UpdateUserAsync(int id, User user)
    {
        
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }


        existingUser.Email = user.Email;
        existingUser.Login = user.Login;
        existingUser.Password = user.Password;
   

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("User not found.");
        }

        
    }
}
