using ToDoList.Core.Entities;

namespace ToDoList.Core.Interfaces
{
    public interface IUserTaskRepository
    {
        Task AddUserTaskAsync(UserTask userTask, CancellationToken cancellationToken);
        Task<UserTask?> GetUserTaskByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<UserTask>> GetAllUsersTasksAsync(CancellationToken cancellationToken);
        void Delete(UserTask userTask);
        void Update(UserTask userTask);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
