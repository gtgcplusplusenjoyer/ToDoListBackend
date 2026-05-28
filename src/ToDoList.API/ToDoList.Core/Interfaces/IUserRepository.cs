using ToDoList.Core.Entities;

namespace ToDoList.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task AddAsync(User user,CancellationToken cancellationToken);
        void Update(User user);
        Task SaveChangesAsync(CancellationToken cancellationToken);

    }
}
