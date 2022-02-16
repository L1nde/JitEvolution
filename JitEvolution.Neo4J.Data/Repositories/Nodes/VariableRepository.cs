using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace JitEvolution.Neo4J.Data.Repositories.Nodes
{
    internal class VariableRepository : NodeRepository<Variable>, IVariableRepository
    {
        public VariableRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override NodeLabelEnum Label => NodeLabelEnum.Variable;

        public async Task<IEnumerable<Result<Variable>>> GetAllAsync(long appId, long classId, string? filter = null)
        {
            var query = (await BaseGetQuery())
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

        public async Task<Result<Variable>?> GetByUsrAsync(string projectId, string usr)
        {
            var model = await (await BaseGetQuery())
                .AndWhere($"app.appKey = '{projectId}'")
                .AndWhere($"variable.usr = '{usr}'")
                .Return(variable => new { Id = variable.Id(), Data = variable.As<Variable>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Variable>(item.Id, item.Data) : null;
        }

        public async Task<Result<Variable>?> GetAsync(long id)
        {
            var model = await (await ClientAsync()).Cypher
                .Match("(variable:Variable)")
                .Where($"ID(variable) = {id}")
                .Return(variable => new { Id = variable.Id(), Data = variable.As<Variable>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Variable>(item.Id, item.Data) : null;
        }

        private async Task<ICypherFluentQuery> BaseGetQuery() =>
            (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(variable:Variable)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(class1)-[:CHANGED_TO]->(:Class)")
                .AndWhere($"not(variable)-[:CHANGED_TO]->(:variable)");
    }
}
