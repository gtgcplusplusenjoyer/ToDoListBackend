using ToDoList.Core.Entities.User;
using ToDoList.Core.Tokens;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken); 
    }
}
