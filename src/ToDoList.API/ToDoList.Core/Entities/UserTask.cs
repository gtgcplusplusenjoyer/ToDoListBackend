using ToDoList.Core.Enums;

namespace ToDoList.Core.Entities
{
    public class UserTask : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ToDoStatus Status { get; set; } = ToDoStatus.Pending;
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
