using Microsoft.Extensions.DependencyInjection;
using ToDoList.Application.Interfaces;
using ToDoList.Application.Mappers;
using ToDoList.Application.Services;

namespace ToDoList.Application.Extensions
{
    public static class AddApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserTaskService, UserTaskService>();
            services.AddAutoMapper(cfg => { }, typeof(UserTaskMapper));

            return services;
        }
    }
}
