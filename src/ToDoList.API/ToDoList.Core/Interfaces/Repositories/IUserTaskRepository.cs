using ToDoList.Core.Entities;
using ToDoList.Core.Models;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IUserTaskRepository : IRepository<UserTask>
    {
        Task<UserTask?> GetUserTaskByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<PagedResult<UserTask>> GetAllUsersTasksAsync(UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken);
        void Update(UserTask userTask);
        void Delete(UserTask userTask);
    }
}
