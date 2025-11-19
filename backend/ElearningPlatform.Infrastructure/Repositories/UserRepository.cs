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

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // EF Core TPT will automatically join tables to get the correct runtime type (Student/Instructor)
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return _context.SaveChangesAsync();
    }
}