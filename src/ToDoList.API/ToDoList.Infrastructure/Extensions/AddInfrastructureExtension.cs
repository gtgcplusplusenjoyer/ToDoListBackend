using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Repositories;

namespace ToDoList.Infrastructure.Extensions
{
    public static class AddInfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserTaskRepository, UserTaskRepository>();
            services.AddDbContext<ToDoListDbContext>(opt=>
            {
                opt.UseNpgsql(configuration.GetConnectionString(nameof(ToDoListDbContext)));
            }
            );
            

            return services;
        }
    }
}
