using ToDoList.Core.Enums;

namespace ToDoList.Application.Dto
{
    public record UserTaskResponseDto(
        Guid Id,
        string Name,
        string Description,
        ToDoStatus Status,
        Priority Priority,
        DateTime DueTime,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
