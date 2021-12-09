using JitEvolution.Core.Services.Identity;
using JitEvolution.Services.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JitEvolution.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
