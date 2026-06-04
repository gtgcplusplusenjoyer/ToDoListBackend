using ToDoList.Core.Entities.User;

namespace ToDoList.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task RevokeAsync(Guid tokenId, CancellationToken cancellationToken = default );
        Task RevokeAllTokensByUserId(Guid userId, CancellationToken cancellationToken=default);
        Task SaveChangesAsync(CancellationToken cancellationToken=default);
    }
}
