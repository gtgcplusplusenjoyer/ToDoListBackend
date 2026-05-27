using ToDoList.Core.Enums;

namespace ToDoList.Application.Dto
{
    public record UserTaskResponseDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public ToDoStatus Status { get; init; }
        public Priority Priority { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
