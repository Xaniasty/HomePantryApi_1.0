namespace HomePantryApi_1._0.Repositories.Interfaces;
using HomePantryApi_1._0.Models;
public interface IUserRepository
{

        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUsersByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
        Task<User?> ValidateUserAsync(string input, string password);


}
