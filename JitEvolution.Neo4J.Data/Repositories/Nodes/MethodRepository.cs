using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;
using Neo4jClient.Cypher;
using Relationship = JitEvolution.Core.Models.Analyzer.Relationship;

namespace JitEvolution.Neo4J.Data.Repositories.Nodes
{
    internal class MethodRepository : NodeRepository<Method>, IMethodRepository
    {
        public MethodRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override NodeLabelEnum Label => NodeLabelEnum.Method;

        public async Task<IEnumerable<Result<Method>>> GetAllAsync(long appId, long classId, string? filter = null)
        {
            var query = (await BaseGetQuery())
                .AndWhere($"ID(class1) = {classId}")
                .AndWhere($"ID(app) = {appId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AndWhere(filter);
            }

            var model = await query
                .Return(method => new { Id = method.Id(), Data = method.As<Method>() })
                .ResultsAsync;
            
            return model.Select(x => new Result<Method>(x.Id, x.Data));
        }

        public async Task<Result<Method>?> GetByUsrAsync(string projectId, string usr)
        {
            var model = await (await BaseGetQuery())
                .AndWhere($"app.appKey = '{projectId}'")
                .AndWhere($"method.usr = '{usr}'")
                .Return(method => new { Id = method.Id(), Data = method.As<Method>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Method>(item.Id, item.Data) : null;
        }

        public async Task<Result<Method>?>GetRelatedMethodAsync(long id, string version)
        {
            var client = await ClientAsync();

            var models = await client.Cypher
                .Match($"(m:{Label})<-[:CLASS_OWNS_METHOD]-(c:{NodeLabelEnum.Class})")
                .Where($"id(m) = {id}")
                .Match($"(m2:{Label})<-[:CLASS_OWNS_METHOD]-(c2:{NodeLabelEnum.Class})")
                .Where($"c.usr = c2.usr and m.usr = m2.usr")
                .AndWhere($"m2.version <> '{version}'")
                .AndWhere($"not(m2)-[:CHANGED_TO]->(:Method)")
                .Return(m2 => new { Id = m2.Id(), Data = m2.As<Method>() })
                .ResultsAsync;

            var item = models.FirstOrDefault();

            return item != null ? new Result<Method>(item.Id, item.Data) : null;
        }

        public Task<IEnumerable<(string ClassUsr, Result<Method> Method)>> GetAllWithClassUsrForAsync(string appKey, string version)
        {
            return GetAllWithClassUsrAsync(appKey, $"n.version = '{version}'");
        }

        public Task<IEnumerable<(string ClassUsr, Result<Method> Method)>> GetAllLatestWithClassUsrAsync(string appKey, string exludeVersion)
        {
            return GetAllWithClassUsrAsync(appKey, $"not(n)-[:CHANGED_TO]->(:{Label}) AND n.version <> '{exludeVersion}'");
        }

        private async Task<IEnumerable<(string ClassUsr, Result<Method> Method)>> GetAllWithClassUsrAsync(string appKey, string filter)
        {
            var client = await ClientAsync();

            var query = client.Cypher.Match($"(n:{Label}{{appKey: '{appKey}'}})<-[:CLASS_OWNS_METHOD]-(p:{NodeLabelEnum.Class})");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(filter);
            }

            var models = await query.Return((n, p) => new { Id = n.Id(), Data = n.As<Method>(), ClassUsr = Return.As<string>("p.usr") })
                .ResultsAsync;

            return models.Select(x => (x.ClassUsr, new Result<Method>(x.Id, x.Data)));
        }
        public async Task<Result<Method>?> GetAsync(long id)
        {
            var model = await (await ClientAsync()).Cypher
                .Match("(method:Method)")
                .Where($"ID(method) = {id}")
                .Return(method => new { Id = method.Id(), Data = method.As<Method>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Method>(item.Id, item.Data) : null;
        }

        public async Task<IEnumerable<Result<Relationship>>> GetAllRelationshipsAsync(long appId, long classId, string? filter = null)
        {
            var query = (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(s:Method)-[r:CALLS]->(e)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(s)-[:CHANGED_TO]->(:Method)")
                .AndWhere($"ID(class1) = {classId}")
                .AndWhere($"ID(app) = {appId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AndWhere(filter);
            }

            var model = await query
                .Return(r => new { Id = r.Id(), Start = Return.As<long>("id(s)"), StartLabel = Return.As<string>("last(labels(s))"), End = Return.As<long>("ID(e)"), EndLabel = Return.As<string>("last(labels(e))"), Type = r.Type() })
                .ResultsAsync;

            return model.Select(x => new Result<Relationship>(x.Id, new Relationship
            {
                Start = (x.Start, Enum.Parse<NodeLabelEnum>(x.StartLabel)),
                End = (x.End, Enum.Parse<NodeLabelEnum>(x.EndLabel)),
                Type = x.Type
            }));
        }

        private async Task<ICypherFluentQuery> BaseGetQuery() =>
            (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(method:Method)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(class1)-[:CHANGED_TO]->(:Class)")
                .AndWhere($"not(method)-[:CHANGED_TO]->(:Method)");
    }
}
