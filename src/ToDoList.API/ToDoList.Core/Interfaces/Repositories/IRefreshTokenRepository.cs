using ToDoList.Core.Tokens;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task RevokeAsync(Guid tokenId, CancellationToken cancellationToken = default );
        Task RevokeAllTokensByUserId(Guid userId, CancellationToken cancellationToken=default);
    }
}
