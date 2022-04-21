﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.PostgreSql;
using JitEvolution.Core.Services.Schedule;
using JitEvolution.Hangfire.Services;

namespace JitEvolution.Hangfire
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("Hangfire"), new PostgreSqlStorageOptions()));

            services.AddHangfireServer();

            services.AddScoped<IScheduler, Scheduler>();
        }
    }
}