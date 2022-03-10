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

        public async Task UpdateAddedOnFor(string version, int addedOn)
        {
            var client = await ClientAsync();

            await client.Cypher.Match($"(a)")
                .Where($"a.version = '{version}'")
                .Set($"a.added_on = {addedOn}")
                .ExecuteWithoutResultsAsync();
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

        public async Task<Result<TEntity>?> GetByIdAndIncomingAsync(long id, string relationshipType)
        {
            var client = await ClientAsync();

            var models = await client.Cypher.Match($"(n:{Label})<-[r:{relationshipType}]-()")
                .Where($"id(n) = {id}")
                .Return(n => new { Id = n.Id(), Data = n.As<TEntity>() })
                .ResultsAsync;

            return models.Select(x => new Result<TEntity>(x.Id, x.Data)).FirstOrDefault();
        }

        public async Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetIncomingRelationshipsAsync(long id)
        {
            var client = await ClientAsync();

            var models = await client.Cypher.Match($"(e:{Label})<-[r]-(s)")
                .Where($"id(e) = {id}")
                .Return(r => new { Id = r.Id(), Start = Return.As<long>("id(s)"), StartLabel = Return.As<string>("last(labels(s))"), End = Return.As<long>("ID(e)"), EndLabel = Return.As<string>("last(labels(e))"), Type = r.Type() })
                .ResultsAsync;

            return models.Select(x => new Result<Core.Models.Analyzer.Relationship>(x.Id, new Core.Models.Analyzer.Relationship
            {
                Start = (x.Start, Enum.Parse<NodeLabelEnum>(x.StartLabel)),
                End = (x.End, Enum.Parse<NodeLabelEnum>(x.EndLabel)),
                Type = x.Type
            }));
        }

        public async Task<IEnumerable<Result<Core.Models.Analyzer.Relationship>>> GetOutgoingRelationshipsAsync(long id)
        {
            var client = await ClientAsync();

            var models = await client.Cypher.Match($"(s:{Label})-[r]->(e)")
                .Where($"id(s) = {id}")
                .Return(r => new { Id = r.Id(), Start = Return.As<long>("id(s)"), StartLabel = Return.As<string>("last(labels(s))"), End = Return.As<long>("ID(e)"), EndLabel = Return.As<string>("last(labels(e))"), Type = r.Type() })
                .ResultsAsync;

            return models.Select(x => new Result<Core.Models.Analyzer.Relationship>(x.Id, new Core.Models.Analyzer.Relationship
            {
                Start = (x.Start, Enum.Parse<NodeLabelEnum>(x.StartLabel)),
                End = (x.End, Enum.Parse<NodeLabelEnum>(x.EndLabel)),
                Type = x.Type
            }));
        }

        public async Task RunQueryAsync(string query)
        {
            await ((IRawGraphClient)await ClientAsync()).ExecuteCypherAsync(new Neo4jClient.Cypher.CypherQuery(query, new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Projection, "neo4j"));
        }
    }
}
