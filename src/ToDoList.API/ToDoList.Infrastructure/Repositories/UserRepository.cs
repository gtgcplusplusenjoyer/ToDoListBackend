using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities.User;
using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.Context;

namespace ToDoList.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoListDbContext _context;
        private readonly DbSet<User> _users;
        public UserRepository(ToDoListDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(_context));
            _users = _context.Set<User>();
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _users.AddAsync(user, cancellationToken);
        }

        public Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return _users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(User user)
        {
            _users.Update(user);
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _users.FirstOrDefaultAsync(_x => _x.Id == id);
        }
    }
}
