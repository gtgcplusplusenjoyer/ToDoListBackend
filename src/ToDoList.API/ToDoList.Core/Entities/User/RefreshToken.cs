namespace ToDoList.Core.Entities.User
{
    public class RefreshToken :BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public Guid UserId { get; set; }
    }
}
