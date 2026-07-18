using ToDoList.Core.Tokens;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task RevokeAsync(Guid tokenId, CancellationToken cancellationToken = default);
        Task RevokeAllTokensByUserId(Guid userId, CancellationToken cancellationToken = default);
    }
}
