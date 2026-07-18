using ToDoList.Core.Entities;

namespace ToDoList.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
