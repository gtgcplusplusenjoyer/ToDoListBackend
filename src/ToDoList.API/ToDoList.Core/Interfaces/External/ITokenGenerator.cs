using ToDoList.Core.Entities.User;

namespace ToDoList.Core.Interfaces.External
{
    public interface ITokenGenerator
    {
        public TokenPair GenerateTokenPair(User user);
        ValueTask<TokenPair?> RefreshTokenPait(string refreshToken, Guid userId, CancellationToken cancellationToken);

    }
}
