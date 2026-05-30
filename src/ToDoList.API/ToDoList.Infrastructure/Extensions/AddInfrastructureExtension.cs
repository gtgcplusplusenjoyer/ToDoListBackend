using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Interfaces.External;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.External;
using ToDoList.Infrastructure.Repositories;
using ToDoList.Infrastructure.Settings;

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
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddDbContext<ToDoListDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString(nameof(ToDoListDbContext)));
            }
            );

            services.Configure<AuthSettings>(opt =>
            {
                configuration.GetSection("Auth");
            });

            return services;
        }
    }
}
