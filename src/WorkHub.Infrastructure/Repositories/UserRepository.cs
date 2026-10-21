using Microsoft.EntityFrameworkCore;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Domain.Entities;
using WorkHub.Infrastructure.Persistence;

namespace WorkHub.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) => _context = context;
        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => _context.Users.Include(u => u.RefreshTokens)
                             .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = email.Trim().ToLowerInvariant();
            return _context.Users.Include(u => u.RefreshTokens)
                                 .FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = email.Trim().ToLowerInvariant();
            return _context.Users.AnyAsync(u => u.Email == normalized, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
            => await _context.Users.AddAsync(user, cancellationToken);

        public Task<User?> GetByRefreshTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    => _context.Users.Include(u => u.RefreshTokens)
                     .FirstOrDefaultAsync(
                         u => u.RefreshTokens.Any(t => t.TokenHash == tokenHash),
                         cancellationToken);
    }
}
