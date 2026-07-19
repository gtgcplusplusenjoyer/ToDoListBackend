using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserTaskService> _logger;
        public UserTaskService(IUserTaskRepository tasks, IMapper mapper, ILogger<UserTaskService> logger)
        {
            _tasks = tasks;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserTaskResponseDto> CreateUserTaskAsync(
            CreateUserTaskDto createUserTaskDto,
            Guid userId,
            CancellationToken cancellationToken)
        {

            _logger.LogInformation(
                "Пользователь {UserId} создает новую задачу: {TaskName}",
                userId,
                createUserTaskDto.Name);

            try
            {
                var task = _mapper.Map<UserTask>(createUserTaskDto);
                task.Id = Guid.NewGuid();
                task.CreatedAt = DateTime.UtcNow;
                task.UpdatedAt = null;
                task.UserId = userId;

                await _tasks.AddAsync(task, cancellationToken);
                await _tasks.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<UserTaskResponseDto>(task);

                _logger.LogInformation(
                    "Задача {TaskId} успешно создана пользователем {UserId}: {TaskName}",
                    task.Id,
                    userId,
                    task.Name);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при создании задачи пользователем {UserId}: {TaskName}",
                    userId,
                    createUserTaskDto.Name);
                throw;
            }
             
        }

        public async Task<bool> DeleteUserTaskAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Пользователь {UserId} удаляет задачу {TaskId}",
                userId,
                id);
            try
            {
                var task = await _tasks.GetUserTaskByIdAndUserIdAsync(id, userId, cancellationToken);

                if (task == null)
                {
                    _logger.LogWarning(
                        "Задача {TaskId} не найдена для пользователя {UserId} при удалении",
                        id,
                        userId);
                    throw new NotFoundException($"UserTask with id {id} not found");
                }

                _tasks.Delete(task);
                await _tasks.SaveChangesAsync(cancellationToken);

                var taskName = task.Name;

                _logger.LogInformation(
                    "Задача {TaskId} ('{TaskName}') удалена пользователем {UserId}",
                    id,
                    taskName,
                    userId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при удалении задачи {TaskId} пользователем {UserId}",
                    id,
                    userId);
                throw;
            }
        }

        public async Task<PagedResult<UserTaskResponseDto>> GetTasksAsync(
            UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                "Пользователь {UserId} запрашивает список задач. Фильтр: {@Filter}, Сортировка: {@Sort}, Страница: {@Page}",
                userId,
                filter,
                sortParams,
                pageParams);

            try
            {
                var pagedResult = await _tasks.GetAllUsersTasksAsync(filter, sortParams, pageParams, userId, cancellationToken);

                var dtos = _mapper.Map<UserTaskResponseDto[]>(pagedResult.Data);

                _logger.LogDebug(
                   "Пользователь {UserId} получил {Count} задач из {Total}",
                   userId,
                   dtos.Length,
                   pagedResult.TotalCount);

                return new PagedResult<UserTaskResponseDto>(dtos, pagedResult.TotalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при получении списка задач пользователем {UserId}",
                    userId);
                throw;
            }
        }

        public async Task<UserTaskResponseDto> GetUserTaskByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {

            _logger.LogDebug(
                "Пользователь {UserId} запрашивает задачу {TaskId}",
                userId,
                id);

            try
            {
                var task = await _tasks.GetUserTaskByIdAndUserIdAsync(id, userId, cancellationToken);

                if (task == null)
                {
                    _logger.LogWarning(
                        "Задача {TaskId} не найдена для пользователя {UserId}",
                        id,
                        userId);
                    throw new NotFoundException($"UserTask with id {id} not found");
                }

                _logger.LogDebug(
                    "Задача {TaskId} получена пользователем {UserId}",
                    id,
                    userId);

                return _mapper.Map<UserTaskResponseDto>(task);
            }
            catch (Exception ex) when (ex is NotFoundException) 
            {
                _logger.LogError(
                    ex,
                    "Ошибка при получении задачи {TaskId} пользователем {UserId}",
                    id,
                    userId);
                throw;
            }
        }

        public async Task<UserTaskResponseDto> UpdateUserTaskAsync(Guid id, 
            UpdateUserTaskDto updateUserTaskDto,
            Guid userId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Пользователь {UserId} обновляет задачу {TaskId}: {TaskName}",
                userId,
                id,
                updateUserTaskDto.Name);

            try
            {
                var task = await _tasks.GetUserTaskByIdAndUserIdAsync(id, userId, cancellationToken);

                if (task == null)
                {
                    _logger.LogWarning(
                        "Задача {TaskId} не найдена для пользователя {UserId} при обновлении",
                        id,
                        userId); 
                    throw new NotFoundException($"UserTask with id {id} not found");
                }

                _mapper.Map(updateUserTaskDto, task);
                task.UpdatedAt = DateTime.UtcNow;
                _tasks.Update(task);

                await _tasks.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Задача {TaskId} обновлена пользователем {UserId}: New name = {NewName}'",
                    id,
                    userId,
                    task.Name);

                return _mapper.Map<UserTaskResponseDto>(task);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Ошибка при обновлении задачи {TaskId} пользователем {UserId}",
                    id,
                    userId);
                throw;
            }
        }
    }
}
