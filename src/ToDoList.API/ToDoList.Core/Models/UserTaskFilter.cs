using ToDoList.Core.Enums;

namespace ToDoList.Core.Models
{
    public class UserTaskFilter
    {
        public string? Name { get; set; }
        public ToDoStatus? Status { get; set; }
        public Priority? Priority { get; set; }
        public DateTime? DueTime { get; set; }


    }
}