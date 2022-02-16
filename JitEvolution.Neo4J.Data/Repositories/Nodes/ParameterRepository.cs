using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace JitEvolution.Neo4J.Data.Repositories.Nodes
{
    internal class ParameterRepository : NodeRepository<Parameter>, IParameterRepository
    {
        public ParameterRepository(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override NodeLabelEnum Label => NodeLabelEnum.Parameter;

        public async Task<IEnumerable<Result<Parameter>>> GetAllAsync(long appId, long classId, long methodId, string? filter = null)
        {
            var query = (await BaseGetQuery())
                .AndWhere($"ID(app) = {appId}")
                .AndWhere($"ID(class1) = {classId}")
                .AndWhere($"ID(method) = {methodId}");

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AndWhere(filter);
            }

            var model = await query
                .Return(parameter => new { Id = parameter.Id(), Data = parameter.As<Parameter>() })
                .ResultsAsync;

            return model.Select(x => new Result<Parameter>(x.Id, x.Data));
        }

        public async Task<Result<Parameter>?> GetByUsrAsync(string projectId, string usr)
        {
            var model = await (await BaseGetQuery())
                .AndWhere($"app.appKey = {projectId}")
                .AndWhere($"variable.usr = '{usr}'")
                .Return(variable => new { Id = variable.Id(), Data = variable.As<Parameter>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Parameter>(item.Id, item.Data) : null;
        }

        public async Task<Result<Parameter>?> GetAsync(long id)
        {
            var model = await (await BaseGetQuery())
                .AndWhere($"ID(variable) = {id}")
                .Return(variable => new { Id = variable.Id(), Data = variable.As<Parameter>() })
                .ResultsAsync;

            var item = model.FirstOrDefault();

            return item != null ? new Result<Parameter>(item.Id, item.Data) : null;
        }

        private async Task<ICypherFluentQuery> BaseGetQuery() =>
            (await ClientAsync()).Cypher
                .Match("(app:App)-[:APP_OWNS_CLASS]->(class1:Class)-[:CLASS_OWNS_METHOD]->(method:Method)-[:HAS_ARGUMENT]->(parameter:Parameter)")
                .Where($"not(app)-[:CHANGED_TO]->(:App)")
                .AndWhere($"not(class1)-[:CHANGED_TO]->(:Class)")
                .AndWhere($"not(method)-[:CHANGED_TO]->(:method)")
                .AndWhere($"not(parameter)-[:CHANGED_TO]->(:parameter)");
    }
}
