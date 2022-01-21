using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer;
using Neo4jClient;

namespace JitEvolution.Neo4J.Data.Repositories
{
    internal class ClassRepository : BaseRepository, IClassRepository
    {
        public ClassRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public async Task<IEnumerable<Result<Class>>> GetAllAsync(long appId, string? filter = null)
        {
            var query = (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"ID(app) = {appId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query.AndWhere(filter);
            }

            var model = await query
                .Return(class1 => new { Id = class1.Id(), Data = class1.As<Class>() })
                .ResultsAsync;

            return model.Select(x => new Result<Class>(x.Id, x.Data));
        }
    }
}
