using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using oauth_service.Models;
using oauth_service.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace oauth_service.Helpers
{
    public static class AuthBuilderExtensions
    {
        public static IServiceCollection AddToken(this IServiceCollection services, IConfigurationSection configKey)
        {
            services.AddTransient<IJwtAuth>(provider => new JwtAuth(configKey.Value));

            return services;
        }

        public static IServiceCollection AddAuthContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<AuthDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        connectionString,
                        new MySqlServerVersion(new Version(10, 5, 8)),
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            )
            .AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddCrypto(this IServiceCollection services, IConfigurationSection hashPassword)
        {
            services.AddTransient<ICrypto>(provider => new Crypto(hashPassword.Value));

            return services;
        }
    }
}