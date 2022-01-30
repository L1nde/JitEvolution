using JitEvolution.Config;
using JitEvolution.Core.Repositories.Analyzer;
using JitEvolution.Neo4J.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neo4jClient;

namespace JitEvolution.Neo4J.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterNeo4JServices(this IServiceCollection services)
        {
            services.AddSingleton<IGraphClient, GraphClient>(services =>
            {
                var config = services.GetRequiredService<IOptions<Configuration>>().Value;
                return new GraphClient(new Uri(config.GraphifyEvolution.Neo4J.Uri), config.GraphifyEvolution.Neo4J.Username, config.GraphifyEvolution.Neo4J.Password);
            });

            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IMethodRepository, MethodRepository>();
            services.AddScoped<IVariableRepository, VariableRepository>();

            return services;
        }
    }
}
