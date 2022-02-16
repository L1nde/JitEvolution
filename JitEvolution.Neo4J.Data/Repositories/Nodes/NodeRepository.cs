using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace JitEvolution.Neo4J.Data.Repositories.Nodes
{
    internal abstract class NodeRepository<TEntity> : BaseRepository, INodeRepository<TEntity> where TEntity : class, INode
    {
        public abstract NodeLabelEnum Label { get; }

        public NodeRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public async Task<Result<TEntity>> MergeAsync(long id, IDictionary<string, object> properties, string version)
        {
            //properties.Add("id", id);
            //var propertiesString = "{ " + string.Join(", ", properties.Select(x => x.Key + $": '{x.Value}'")) + " }";

            //var client = await ClientAsync();

            //var filter = properties.ContainsKey("usr")
            //    ? $"{{usr: '{properties["usr"]}', appKey: '{properties["appKey"]}'}}"
            //    : $"{{appKey: '{properties["appKey"]}'}}";

            //var models = await client.Cypher.Merge($"(n:{Label}{{id: '{id}'}})")
            //    .OnCreate().Set($"n = {propertiesString}")
            //    .OnMatch().Set($"n = {propertiesString}")
            //    .Return(n => new { Id = n.Id(), Data = n.As<TEntity>() })
            //    .ResultsAsync;

            //var model = models.Select(x => new Result<TEntity>(x.Id, x.Data)).FirstOrDefault();

            //var existings = await client.Cypher.Match($"(n:{Label}{filter})")
            //    .Where($"not(n)-[:CHANGED_TO]->(:{Label})")
            //    .AndWhere($"id(n) <> {model.Id}")
            //    .Return(n => new { Id = n.Id(), Data = n.As<TEntity>() })
            //    .ResultsAsync;

            //var existing = existings.Select(x => new Result<TEntity>(x.Id, x.Data)).FirstOrDefault();

            //if (existing != null)
            //{
            //    await client.Cypher
            //        .Match($"(n:{Label})")
            //        .Where($"id(n) = {existing.Id}")
            //        .Match($"(n2:{Label})")
            //        .Where($"id(n2) = {model.Id}")
            //        .Merge("(n)-[r:CHANGED_TO]->(n2)")
            //        .Return(r => r.Id())
            //        .ResultsAsync;
            //}

            //return model;

            properties.Add("id", id);
            properties.Add("version", version);
            var propertiesString = "{ " + string.Join(", ", properties.Select(x => x.Key + $": '{x.Value}'")) + " }";

            var client = await ClientAsync();

            var models = await client.Cypher.Merge($"(n:{Label}{{id: '{id}'}})")
                .OnCreate().Set($"n = {propertiesString}")
                .OnMatch().Set($"n = {propertiesString}")
                .Return(n => new { Id = n.Id(), Data = n.As<TEntity>() })
                .ResultsAsync;

            var model = models.Select(x => new Result<TEntity>(x.Id, x.Data)).FirstOrDefault();

            return model;
        }

        public Task<IEnumerable<Result<TEntity>>> GetAllForAsync(string appKey, string version)
        {
            return GetAllAsync(appKey, $"n.version = '{version}'");
        }

        public Task<IEnumerable<Result<TEntity>>> GetAllLatestAsync(string appKey, string exludeVersion)
        {
            return GetAllAsync(appKey, $"not(n)-[:CHANGED_TO]->(:{Label}) AND n.version <> '{exludeVersion}'");
        }

        public async Task AddRelationshipAsync(string fromId, string toId, string type)
        {
            var client = await ClientAsync();

            await client.Cypher.Match($"(a), (c)")
                .Where($"a.id = '{fromId}' and c.id = '{toId}'")
                .Merge($"(a)-[r:{type}]->(c)")
                .Return(r => new { Id = r.Id() })
                .ResultsAsync;
        }

        public async Task AddRelationshipAsync(long fromId, long toId, string type)
        {
            var client = await ClientAsync();

            await client.Cypher.Match($"(a), (c)")
                .Where($"id(a) = {fromId} and id(c) = {toId}")
                .Merge($"(a)-[r:{type}]->(c)")
                .Return(r => new { Id = r.Id() })
                .ResultsAsync;
        }

        public async Task DeleteWithRelationshipAsync(long id)
        {
            var client = await ClientAsync();

            await client.Cypher.Match($"(a:{Label})")
                .Where($"id(a) = {id}")
                .DetachDelete("a")
                .ExecuteWithoutResultsAsync();
        }

        public async Task DeleteRelationshipAsync(long id)
        {
            var client = await ClientAsync();

            await client.Cypher.Match($"()-[r]->()")
                .Where($"id(r) = {id}")
                .DetachDelete("r")
                .ExecuteWithoutResultsAsync();
        }

        private async Task<IEnumerable<Result<TEntity>>> GetAllAsync(string appKey, string? filter = null)
        {
            var client = await ClientAsync();

            var query = client.Cypher.Match($"(n:{Label}{{appKey: '{appKey}'}})");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(filter);
            }

            var models = await query.Return(n => new { Id = n.Id(), Data = n.As<TEntity>() })
                .ResultsAsync;

            return models.Select(x => new Result<TEntity>(x.Id, x.Data));
        }

        public async Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetIncomingRelationshipsAsync(long id)
        {
            var client = await ClientAsync();

            var models = await client.Cypher.Match($"(n:{Label})<-[r]-()")
                .Where($"id(n) = {id}")
                .Return(r => new { Id = r.Id(), Start = Return.As<long>("ID(startNode(r))"), End = Return.As<long>("ID(endNode(r))"), Type = r.Type() })
                .ResultsAsync;

            return models.Select(x => new Result<Core.Models.Analyzer.Relationship>(x.Id, new Core.Models.Analyzer.Relationship
            {
                Start = x.Start,
                End = x.End,
                Type = x.Type
            }));
        }

        public async Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetOutgoingRelationshipsAsync(long id)
        {
            var client = await ClientAsync();

            var models = await client.Cypher.Match($"(n:{Label})-[r]->()")
                .Where($"id(n) = {id}")
                .Return(r => new { Id = r.Id(), Start = Return.As<long>("ID(startNode(r))"), End = Return.As<long>("ID(endNode(r))"), Type = r.Type() })
                .ResultsAsync;

            return models.Select(x => new Result<Core.Models.Analyzer.Relationship>(x.Id, new Core.Models.Analyzer.Relationship
            {
                Start = x.Start,
                End = x.End,
                Type = x.Type
            }));
        }

        public async Task RunQueryAsync(string query)
        {
            await ((IRawGraphClient)await ClientAsync()).ExecuteCypherAsync(new Neo4jClient.Cypher.CypherQuery(query, new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Projection, "neo4j"));
        }
    }
}
