using ToDoList.Core.Enums;

namespace ToDoList.Application.Dto
{
    public record CreateUserTaskDto(
        string Name,
        string Description,
        ToDoStatus Status,
        Priority Priority,
        DateTime DueDate);
}
