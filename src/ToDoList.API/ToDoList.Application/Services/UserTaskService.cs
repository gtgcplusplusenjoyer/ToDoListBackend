using AutoMapper;
using ToDoList.Application.Dto.UserTask;
using ToDoList.Application.Interfaces;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces.Repositories;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Exceptions;

namespace ToDoList.Application.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserTaskRepository _tasks;
        private readonly IMapper _mapper;
        public UserTaskService(IUserTaskRepository tasks, IMapper mapper)
        {
            _tasks = tasks;
            _mapper = mapper;
        }

        public async Task<UserTaskResponseDto> CreateUserTaskAsync(
            CreateUserTaskDto createUserTaskDto,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var task = _mapper.Map<UserTask>(createUserTaskDto);
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = null;
            task.UserId = userId;

            await _tasks.AddUserTaskAsync(task, cancellationToken);
            await _tasks.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UserTaskResponseDto>(task);

            return response;
        }

        public async Task<bool> DeleteUserTaskAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var task = await _tasks.GetUserTaskByIdAndUserIdAsync(id, userId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException($"UserTask with id {id} not found");
            }

            _tasks.Delete(task);
            await _tasks.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<PagedResult<UserTaskResponseDto>> GetTasksAsync(
            UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var pagedResult = await _tasks.GetAllUsersTasksAsync(filter, sortParams, pageParams, userId, cancellationToken);

            var dtos = _mapper.Map<UserTaskResponseDto[]>(pagedResult.Data);

            return new PagedResult<UserTaskResponseDto>(dtos, pagedResult.TotalCount);
        }

        public async Task<UserTaskResponseDto> GetUserTaskByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var task = await _tasks.GetUserTaskByIdAndUserIdAsync(id, userId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException($"UserTask with id {id} not found");
            }

            return _mapper.Map<UserTaskResponseDto>(task);
        }

        public async Task<UserTaskResponseDto> UpdateUserTaskAsync(Guid id, 
            UpdateUserTaskDto updateUserTaskDto,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var task = await _tasks.GetUserTaskByIdAndUserIdAsync(id, userId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException($"UserTask with id {id} not found");
            }

            _mapper.Map(updateUserTaskDto, task);
            task.UpdatedAt = DateTime.UtcNow;
            _tasks.Update(task);

            await _tasks.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserTaskResponseDto>(task);
        }
    }
}
