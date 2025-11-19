using ElearningPlatform.Core.Entities;

namespace ElearningPlatform.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> ExistsAsync(string email);
    Task<User> AddAsync(User user);
}