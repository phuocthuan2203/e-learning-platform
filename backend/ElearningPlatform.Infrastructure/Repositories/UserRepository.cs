using ElearningPlatform.Core.Entities;
using ElearningPlatform.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElearningPlatform.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}