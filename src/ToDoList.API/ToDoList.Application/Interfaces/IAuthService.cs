using ToDoList.Application.Dto.User;

namespace ToDoList.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);
        Task LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);
    }
}
