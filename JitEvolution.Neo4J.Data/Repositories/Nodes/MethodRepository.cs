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
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(method:Method)-[relationship1:CALLS]->()")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(method)-[:CHANGED_TO]->(:Method)")
                .AndWhere($"ID(class1) = {classId}")
                .AndWhere($"ID(app) = {appId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AndWhere(filter);
            }

            var model = await query
                .Return(relationship1 => new { Id = relationship1.Id(), Start = Return.As<long>("ID(startNode(relationship1))"), End = Return.As<long>("ID(endNode(relationship1))"), Type = relationship1.Type() })
                .ResultsAsync;

            return model.Select(x => new Result<Relationship>(x.Id, new Relationship
            {
                Start = x.Start,
                End = x.End,
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
