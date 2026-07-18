using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Interfaces.Repositories;
using ToDoList.Core.Tokens;
using ToDoList.Infrastructure.Context;

namespace ToDoList.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DbSet<RefreshToken> _rTokens;
        private readonly ToDoListDbContext _context;

        public RefreshTokenRepository(ToDoListDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _rTokens = _context.Set<RefreshToken>();
        }

        public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return _rTokens.FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        }

        public async Task RevokeAsync(Guid tokenId,CancellationToken cancellationToken)
        {
            var token = await _rTokens.FindAsync(tokenId, cancellationToken);

            if (token != null)
            {
                token.IsRevoked = true;
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeAllTokensByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var userTokens = await _rTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
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

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _rTokens.AddAsync(refreshToken, cancellationToken);
        }
    }
}
