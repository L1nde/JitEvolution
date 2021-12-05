using JitEvolution.Core.Services;
using JitEvolution.Services.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JitEvolution.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
