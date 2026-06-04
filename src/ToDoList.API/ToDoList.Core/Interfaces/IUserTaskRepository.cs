using ToDoList.Core.Entities;
using ToDoList.Core.Models;

namespace ToDoList.Core.Interfaces
{
    public interface IUserTaskRepository
    {
        Task AddUserTaskAsync(UserTask userTask, CancellationToken cancellationToken);
        Task<UserTask?> GetUserTaskByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<UserTask?> GetUserTaskByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<PagedResult<UserTask>> GetAllUsersTasksAsync(UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken);
        void Delete(UserTask userTask);
        void Update(UserTask userTask);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
