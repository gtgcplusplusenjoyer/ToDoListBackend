using AutoMapper;
using ToDoList.Application.Dto;
using ToDoList.Application.Exceptions;
using ToDoList.Application.Interfaces;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

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

        public async Task<UserTaskResponseDto> CreateUserTaskAsync(CreateUserTaskDto createUserTaskDto, CancellationToken cancellationToken)
        {
            var task = _mapper.Map<UserTask>(createUserTaskDto);
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = null;

            await _tasks.AddUserTaskAsync(task,cancellationToken);
            await _tasks.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UserTaskResponseDto>(task);
            return response;
        }

        public async Task<bool> DeleteUserTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            var task = await _tasks.GetUserTaskByIdAsync(id,cancellationToken);
            
            if(task == null)
            {
                throw new NotFoundException("UserTask not found");
            }

            _tasks.Delete(task);
            await _tasks.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<PagedResult<UserTaskResponseDto>> GetUsersAsync(UserTaskFilter filter, SortParams sortParams, PageParams pageParams, CancellationToken cancellationToken)
        {
            var pagedResult = await _tasks.GetAllUsersTasksAsync(filter,sortParams, pageParams, cancellationToken);

            var dtos = _mapper.Map<UserTaskResponseDto[]>(pagedResult.Data);

            return new PagedResult<UserTaskResponseDto>(dtos, pagedResult.TotalCount);
        }

        public async Task<UserTaskResponseDto?> GetUserTaskByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var task = await _tasks.GetUserTaskByIdAsync(id, cancellationToken);

            if(task == null)
            {
                return null;
            }

            return _mapper.Map<UserTaskResponseDto>(task);
        }

        public async Task<UserTaskResponseDto?> UpdateUserTaskAsync(Guid id, UpdateUserTaskDto updateUserTaskDto, CancellationToken cancellationToken)
        {
            var task = await _tasks.GetUserTaskByIdAsync(id, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException("UserTask not found");
            }

            _mapper.Map(updateUserTaskDto, task);
            task.UpdatedAt = DateTime.UtcNow;
            _tasks.Update(task);

            await _tasks.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserTaskResponseDto>(task);
        }
    }
}
