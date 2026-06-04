using ToDoList.Application.Dto.UserTask;
using ToDoList.Core.Models;

namespace ToDoList.Application.Interfaces
{
    public interface IUserTaskService
    {
        Task<UserTaskResponseDto> CreateUserTaskAsync(CreateUserTaskDto createUserTaskDto, CancellationToken cancellationToken);
        Task<UserTaskResponseDto> UpdateUserTaskAsync(Guid id, UpdateUserTaskDto updateUserTaskDto, CancellationToken cancellationToken);
        Task<bool> DeleteUserTaskAsync(Guid id, CancellationToken cancellationToken);
        Task<UserTaskResponseDto> GetUserTaskByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedResult<UserTaskResponseDto>> GetTasksAsync(UserTaskFilter filter, SortParams sortParams, PageParams pageParams, CancellationToken cancellationToken);
    }
}
