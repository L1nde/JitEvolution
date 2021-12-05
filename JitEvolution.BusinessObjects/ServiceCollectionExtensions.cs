using JitEvolution.BusinessObjects.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JitEvolution.BusinessObjects
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterBusinessObjects(this IServiceCollection services)
        {
            services.AddScoped<CurrentUser>();

            return services;
        }
    }
}
