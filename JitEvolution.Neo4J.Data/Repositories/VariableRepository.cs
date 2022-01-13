using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer;
using Neo4jClient;

namespace JitEvolution.Neo4J.Data.Repositories
{
    internal class VariableRepository : BaseRepository, IVariableRepository
    {
        public VariableRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public async Task<IEnumerable<Result<Variable>>> GetAll(long appId, long classId, string? filter = null)
        {
            var query = (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(variable:Variable)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(variable)-[:CHANGED_TO]->(:variable)")
                .AndWhere($"ID(class1) = {classId}")
                .AndWhere($"ID(app) = {appId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AndWhere(filter);
            }

            var model = await query
                .Return(variable => new { Id = variable.Id(), Data = variable.As<Variable>() })
                .ResultsAsync;

            return model.Select(x => new Result<Variable>(x.Id, x.Data));
        }
    }
}
