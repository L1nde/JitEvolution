using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;
using Neo4jClient.Cypher;

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

        public async Task<IEnumerable<int>> GetAppVersionNumbersAsync(string projectId)
        {
            var client = await ClientAsync();

            var models = await client.Cypher
                .Match("(app:App)")
                .Where($"app.appKey = '{projectId}'")
                .OptionalMatch("appPath=(app)<-[:CHANGED_TO*]-(:App)")
                .With((app, appPath) => new
                {
                    app = Return.As<App>("app{.*, version_number: coalesce(max(length(appPath)), 0)}"),
                })
                .Return(app => new { VersionNumber = Return.As<int>("app.version_number") })
                .ResultsAsync;

            return models.Select(x => x.VersionNumber).OrderByDescending(x => x);
        }

        public async Task<IEnumerable<App>> GetResultAsync(string projectId, int? versionNumber)
        {
            var client = await ClientAsync();

            if(!versionNumber.HasValue)
            {
                versionNumber = (await GetAppVersionNumbersAsync(projectId)).OrderByDescending(x => x).FirstOrDefault();

                if (versionNumber == null)
                {
                    return Enumerable.Empty<App>();
                }
            }

            var models = await client.Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(method:Method)")
                .Where($"app.appKey = '{projectId}'")
                .OptionalMatch("(class1)-[CLASS_OWNS_VARIABLE]->(variable:Variable)")
                .OptionalMatch("appPath=(app)<-[:CHANGED_TO*]-(:App)")
                .OptionalMatch("classPath=(class1)<-[:CHANGED_TO*]-(:Class)")
                .OptionalMatch("methodPath=(method)<-[:CHANGED_TO*]-(:Method)")
                .OptionalMatch("variablePath=(variable)<-[:CHANGED_TO*]-(:Variable)")
                .OptionalMatch("(method)-[r2:CALLS]->()")
                .OptionalMatch("(method)-[r:USES]->()")
                .With((app, appPath, class1, classPath, method, methodPath, variable, variablePath, r, r2) => new 
                {
                    app = Return.As<App>("app{.*, version_number: coalesce(max(length(appPath)), 0)}"),
                    class1 = Return.As<Class>("class1{.*, version_number: coalesce(max(length(classPath)), 0)}"),
                    method = Return.As<Method>("method{.*, version_number: coalesce(max(length(methodPath)), 0), calls: collect(distinct endnode(r).id), uses: collect(distinct endnode(r2).id)}"),
                    variable = Return.As<Variable>("variable{.*, version_number: coalesce(max(length(variablePath)), 0)}")
                })
                .Where($"app.version_number = {versionNumber}")
                .With((app, class1, method, variable) => new { app, class1, methods = method.CollectAsDistinct<Method>(), variables = variable.CollectAsDistinct<Variable>() })
                .With((app, class1, methods, variables) => new { app, classes = Return.As<Class>("collect(class1 {.*, methods, variables})") })
                .With((app, classes) => new { apps = Return.As<App>("collect(app {.*, classes})") })
                .Return((apps) => new { Apps = apps.As<App[]>() })
                .ResultsAsync;

            return models.FirstOrDefault()?.Apps ?? Enumerable.Empty<App>();
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

        public async Task<IEnumerable<Core.Models.Analyzer.Relationship>> GetRelationshipsAsync(string projectId)
        {
            var client = await ClientAsync();

            var models = await client.Cypher
                .Match($"(a{{appKey: '{projectId}'}})-[r]->(b{{appKey: '{projectId}'}})")
                .Where($"not(a)-[:CHANGED_TO]->()")
                .AndWhere($"not(b)-[:CHANGED_TO]->()")
                .AndWhere($"a.appKey = '{projectId}' OR b.appKey = '{projectId}'")
                .Return((r) => new { Relationship = r.As<Core.Models.Analyzer.Relationship>() })
                .ResultsAsync;

            return models.Select(x => x.Relationship);
        }
    }
}
