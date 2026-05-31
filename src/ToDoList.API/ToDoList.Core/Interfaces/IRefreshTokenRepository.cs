using ToDoList.Core.Entities.User;

namespace ToDoList.Core.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task CreateAsync(RefreshToken refreshToken);
        Task RevokeAsync(Guid tokenId);
        Task RevokeAllTokensByUserId(Guid userId,CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);   
    }
}
