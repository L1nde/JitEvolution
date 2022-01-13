using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Repositories.Analyzer;
using JitEvolution.Core.Repositories.Identity;
using JitEvolution.Data.Repositories;
using JitEvolution.Data.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JitEvolution.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<JitEvolutionDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(configuration.GetConnectionString("JitEvolution")));
        }

        public static void RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<JitEvolutionDbContext>()
                .AddDefaultTokenProviders();
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
