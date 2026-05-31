using ToDoList.Application.Dto.User;
using ToDoList.Core.Entities.User;

namespace ToDoList.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
        Task<AuthResult> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);
        Task<AuthResult> LogoutAsync(Guid id, CancellationToken cancellationToken);
        Task<AuthResult> RefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}
