using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer;
using Neo4jClient;

namespace JitEvolution.Neo4J.Data.Repositories
{
    internal class AppRepository : BaseRepository, IAppRepository
    {
        public AppRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public async Task<IEnumerable<Result<App>>> GetAllAsync()
        {
            var model = await (await ClientAsync()).Cypher.Match("(app:App)")
                .Where("not(app)-[:CHANGED_TO]-(:App)")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            return model.Select(x => new Result<App>(x.Id, x.Data));
        }

        public async Task<Result<App>> GetAsync(string projectId)
        {
            var model = await(await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"app.appKey = '{projectId}'")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return new Result<App>(item.Id, item.Data);
        }
    }
}
