using ToDoList.Application.Dto.UserTask;
using ToDoList.Core.Models;

namespace ToDoList.Application.Interfaces
{
    public interface IUserTaskService
    {
        Task<UserTaskResponseDto> CreateUserTaskAsync(CreateUserTaskDto createUserTaskDto,
            Guid userId,
            CancellationToken cancellationToken);
        Task<UserTaskResponseDto> UpdateUserTaskAsync(Guid id,
            UpdateUserTaskDto updateUserTaskDto,
            Guid UserId,
            CancellationToken cancellationToken);
        Task<bool> DeleteUserTaskAsync(Guid id,
            Guid userId,
            CancellationToken cancellationToken);
        Task<UserTaskResponseDto> GetUserTaskByIdAsync(Guid id,
            Guid userId, 
            CancellationToken cancellationToken);
        Task<PagedResult<UserTaskResponseDto>> GetTasksAsync(UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken);
    }
}
