﻿using Microsoft.Extensions.DependencyInjection;

namespace JitEvolution.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
