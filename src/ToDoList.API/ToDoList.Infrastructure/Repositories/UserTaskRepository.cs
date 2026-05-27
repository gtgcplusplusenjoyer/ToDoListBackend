using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.Context;

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

        public async Task<List<UserTask>> GetAllUsersTasksAsync(CancellationToken cancellationToken)
        {
            return await _tasks
                .OrderBy(t=>t.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserTask?> GetUserTaskByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _tasks.FirstOrDefaultAsync(t=>t.Id == id,cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(UserTask userTask)
        {
            _tasks.Update(userTask);
        }
    }
}
