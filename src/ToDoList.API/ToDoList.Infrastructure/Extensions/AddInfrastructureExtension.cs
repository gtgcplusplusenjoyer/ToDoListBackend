using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Interfaces.External;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.External;
using ToDoList.Infrastructure.Repositories;

namespace ToDoList.Infrastructure.Extensions
{
    public static class AddInfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserTaskRepository, UserTaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddDbContext<ToDoListDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString(nameof(ToDoListDbContext)));
            }
            );


            return services;
        }
    }
}
