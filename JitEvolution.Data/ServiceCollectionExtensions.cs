using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Repositories.IDE;
using JitEvolution.Core.Repositories.Identity;
using JitEvolution.Core.Repositories.Queue;
using JitEvolution.Data.Repositories;
using JitEvolution.Data.Repositories.IDE;
using JitEvolution.Data.Repositories.Identity;
using JitEvolution.Data.Repositories.Queue;
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
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IQueueItemRepository, QueueItemRepository>();

            return services;
        }
    }
}
