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

        public async Task<IEnumerable<Result<App>>> GetAll()
        {
            var model = await (await ClientAsync()).Cypher.Match("(app:App)")
                .Where("not(app)-[:CHANGED_TO]-(:App)")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            return model.Select(x => new Result<App>(x.Id, x.Data));
        }

        public async Task<Result<App>> Get(long appId)
        {
            var model = await(await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"ID(app) = {appId}")
                .Return(app => new { Id = app.Id(), Data = app.As<App>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            //var model = (await ClientAsync()).Cypher
            //    .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(method:Method)")
            //    .Where($"not(app)-[:CHANGED_TO]->(:App)")
            //    .AndWhere("not(method)-[:CHANGED_TO]->(:Method)")
            //    .AndWhere("method.start_line <> 0 and method.end_line <> 0")
            //    .AndWhere($"ID(app) = {appId}")
            //    .Return((app, class1) => new { Id = app.Id(), App = app.As<App>(), Class = class1.As<Class>() })
            //    .ResultsAsync;

            //var item = model.FirstOrDefault();

            return new Result<App>(item.Id, item.Data);
        }
    }
}
