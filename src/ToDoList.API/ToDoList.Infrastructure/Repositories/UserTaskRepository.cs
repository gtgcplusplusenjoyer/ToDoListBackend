using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces.Repositories;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Extensions;

namespace ToDoList.Infrastructure.Repositories
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly DbSet<UserTask> _tasks;
        private readonly ToDoListDbContext _context;

        public UserTaskRepository(ToDoListDbContext context)
        {
            _context = context ?? throw new ArgumentNullException("Context not found");
            _tasks = _context.Set<UserTask>();
        }

        public async Task AddUserTaskAsync(UserTask userTask, CancellationToken cancellationToken)
        {
            await _tasks.AddAsync(userTask, cancellationToken);
        }

        public void Delete(UserTask userTask)
        {
            _tasks.Remove(userTask);
        }

        public async Task<PagedResult<UserTask>> GetAllUsersTasksAsync(UserTaskFilter filter,
            SortParams sortParams,
            PageParams pageParams,
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await _tasks
                .AsNoTracking()
                .Where(t =>t.UserId == userId)
                .Filter(filter)
                .Sort(sortParams)
                .ToPagedAsync(pageParams);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(UserTask userTask)
        {
            _tasks.Update(userTask);
        }

        public async Task<UserTask?> GetUserTaskByIdAndUserIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            return await _tasks.FirstOrDefaultAsync(t=>t.Id == id && t.UserId == userId, cancellationToken);
        }
    }
}
