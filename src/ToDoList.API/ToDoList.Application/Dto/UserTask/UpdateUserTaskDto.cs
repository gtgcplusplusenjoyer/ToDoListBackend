using ToDoList.Core.Enums;

namespace ToDoList.Application.Dto.UserTask
{
    public record UpdateUserTaskDto(
        string Name,
        string Description,
        ToDoStatus Status,
        Priority Priority,
        DateTime DueDate);
}
