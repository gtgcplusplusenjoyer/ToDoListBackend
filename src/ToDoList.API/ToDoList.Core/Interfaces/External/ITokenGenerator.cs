using ToDoList.Core.Entities.User;
using ToDoList.Core.Tokens;

namespace ToDoList.Core.Interfaces.External
{
    public interface ITokenGenerator
    {
        public TokenPair GenerateTokenPair(User user);

    }
}
