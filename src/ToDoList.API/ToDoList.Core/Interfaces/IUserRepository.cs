using ToDoList.Core.Entities.User;

namespace ToDoList.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
        void Update(User user);
        Task SaveChangesAsync(CancellationToken cancellationToken);

    }
}
