using ToDoList.Core.Interfaces.External;

namespace ToDoList.Infrastructure.External
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            return hash;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
