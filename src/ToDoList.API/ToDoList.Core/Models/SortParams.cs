using ToDoList.Core.Enums;

namespace ToDoList.Core.Models
{
    public class SortParams
    {
        public string? OrderBy { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}
