using ToDoList.Core.Entities;
using ToDoList.Core.Models;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IUserTaskRepository : IRepository<UserTask>
    {
        Task AddUserTaskAsync(UserTask userTask, CancellationToken cancellationToken);
        Task<UserTask?> GetUserTaskByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<PagedResult<UserTask>> GetAllUsersTasksAsync(UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken);
    }
}
