using FluentValidation;
using ToDoList.Application.Dto.UserTask;
using ToDoList.Core.Enums;

namespace ToDoList.Application.Validators.UserTask
{
    public class UpdateUserTaskDtoValidator : AbstractValidator<UpdateUserTaskDto>
    {
        public UpdateUserTaskDtoValidator()
        {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("Task name is required")
                   .MaximumLength(100).WithMessage("Task name must not exceed 100 characters")
                   .MinimumLength(3).WithMessage("Task name must be at least 3 characters long");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid task status");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value");

            RuleFor(x => x.DueDate)
                .Must(BeAValidDate).WithMessage("Please provide a valid date")
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future");

            RuleFor(x => x)
                .Must(x => !(x.Status == ToDoStatus.Completed && x.DueDate > DateTime.UtcNow))
                .WithMessage("Cannot complete a task whose due date hasn't arrived yet");

            RuleFor(x => x)
                .Must(x => !(x.Priority == Priority.High && x.Name.Length < 5))
                .WithMessage("High priority tasks must have a name with at least 5 characters");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default(DateTime) && date > DateTime.MinValue;
        }
    }
}
