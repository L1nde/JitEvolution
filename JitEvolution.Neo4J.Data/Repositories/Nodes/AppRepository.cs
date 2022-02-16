using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;

namespace JitEvolution.Neo4J.Data.Repositories.Nodes
{
    internal class AppRepository : NodeRepository<App>, IAppRepository
    {
        public AppRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override NodeLabelEnum Label => NodeLabelEnum.App;

        public async Task<IEnumerable<Result<App>>> GetAllAsync()
        {
            var model = await (await ClientAsync()).Cypher.Match("(app:App)")
                .Where("not(app)-[:CHANGED_TO]-(:App)")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            return model.Select(x => new Result<App>(x.Id, x.Data));
        }

        public async Task<Result<App>?> GetByAppKeyAsync(string projectId)
        {
            var model = await (await ClientAsync()).Cypher
                .Match("(app:App)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"app.appKey = '{projectId}'")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            var item = model?.FirstOrDefault();

            return item != null ? new Result<App>(item.Id, item.Data) : null;
        }

        public async Task<Result<App>?> GetAsync(long id)
        {
            var model = await (await ClientAsync()).Cypher
                .Match("(app:App)")
                .Where($"ID(app) = {id}")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            var item = model?.FirstOrDefault();

            return item != null ? new Result<App>(item.Id, item.Data) : null;
        }
    }
}
