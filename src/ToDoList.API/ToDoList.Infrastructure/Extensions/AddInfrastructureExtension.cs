using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

            services.AddAuthExtension(configuration);

            return services;
        }

        public static IServiceCollection AddAuthExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection("Auth").Get<AuthSettings>();

            services.Configure<AuthSettings>(configuration.GetSection("Auth"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authSettings?.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authSettings?.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(authSettings?.Secret ?? "fallback-secret-key-32-chars-long!!!")),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

    }
}
