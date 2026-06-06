using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using ToDoList.Application.Dto.User;
using ToDoList.Application.Dto.UserTask;
using ToDoList.Application.Validators.User;
using ToDoList.Application.Validators.UserTask;

namespace ToDoList.Application.Extensions
{
    public static class AddValidationExtension
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<LoginUserDto>, LoginUserDtoValidator>();
            services.AddScoped<IValidator<CreateUserTaskDto>, CreateUserTaskDtoValidator>();
            services.AddScoped<IValidator<UpdateUserTaskDto>, UpdateUserTaskDtoValidator>();
            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
