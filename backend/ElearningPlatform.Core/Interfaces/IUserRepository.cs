using ElearningPlatform.Core.Entities;

namespace ElearningPlatform.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
}