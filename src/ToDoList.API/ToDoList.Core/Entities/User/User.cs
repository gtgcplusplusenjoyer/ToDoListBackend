namespace ToDoList.Core.Entities.User
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
