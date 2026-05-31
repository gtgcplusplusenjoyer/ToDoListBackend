using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities.User;
using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.Context;

namespace ToDoList.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> _rTokens;
        private readonly ToDoListDbContext _context;

        public RefreshTokenRepository(ToDoListDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
            _rTokens = _context.Set<RefreshToken>();
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            await _rTokens.AddAsync(refreshToken);
        }

        public Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return _rTokens.FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task RevokeAsync(Guid tokenId)
        {
            var token = await _rTokens.FindAsync(tokenId);

            if (token != null)
            {
                token.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeAllTokensByUserId(Guid userId,CancellationToken cancellationToken)
        {
            var userTokens = await _rTokens
                .Where(rt=>rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in userTokens)
            {
                token.IsRevoked = true;
            }

            if (userTokens.Any())
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
