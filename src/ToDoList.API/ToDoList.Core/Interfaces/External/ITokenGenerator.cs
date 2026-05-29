using ToDoList.Core.Entities.User;

namespace ToDoList.Core.Interfaces.External
{
    public interface ITokenGenerator
    {
        public TokenPair GenerateTokenPair(User user);
        ValueTask<TokenPair?> RefreshTokenPair(string refreshToken, Guid userId, CancellationToken cancellationToken);

    }
}
