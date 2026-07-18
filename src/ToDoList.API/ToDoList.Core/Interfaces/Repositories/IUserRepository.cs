using ToDoList.Core.Entities.User;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
    }
}
