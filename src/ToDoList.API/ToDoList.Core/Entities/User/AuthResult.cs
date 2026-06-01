namespace ToDoList.Core.Entities.User
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public TokenPair? TokenPair { get; set; }
        public Guid? UserId { get; set; }

        public static AuthResult Success(TokenPair tokenPair, Guid userId) =>
            new AuthResult { IsSuccess = true, TokenPair = tokenPair, UserId = userId };
        public static AuthResult Success(Guid userId) =>
            new AuthResult { IsSuccess = true, UserId = userId };
        public static AuthResult Failure(string message) => 
            new AuthResult { IsSuccess = false, ErrorMessage = message };


    }
}
