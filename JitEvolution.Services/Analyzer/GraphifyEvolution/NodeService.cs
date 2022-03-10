using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer;
using JitEvolution.Core.Models.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using JitEvolution.Core.Services.Analyzer.GraphifyEvolution;

namespace JitEvolution.Services.Analyzer.GraphifyEvolution
{
    public class NodeService : INodeService
    {
        private readonly IAppRepository _appRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly IVariableRepository _variableRepository;
        private readonly IParameterRepository _parameterRepository;
        //private readonly IExternalRepository _externalRepository;

        public NodeService(IAppRepository appRepository,
            IClassRepository classRepository,
            IMethodRepository methodRepository,
            IVariableRepository variableRepository,
            IParameterRepository parameterRepository)
            //IExternalRepository externalRepository)
        {
            _appRepository = appRepository;
            _classRepository = classRepository;
            _methodRepository = methodRepository;
            _variableRepository = variableRepository;
            _parameterRepository = parameterRepository;
            //_externalRepository = externalRepository;
        }

        public async Task<long> CreateOrUpdateAsync(NodeDto node, string appKey, string version)
        {
            EnsureAppKey(node, appKey);

            var nodeId = (node.Label) switch
            {
                NodeLabelEnum.App => (await CreateOrUpdateAppAsync(node, version)).Id,
                NodeLabelEnum.Class => (await CreateOrUpdateClassAsync(node, version)).Id,
                NodeLabelEnum.Method => (await CreateOrUpdateMethodAsync(node, version)).Id,
                NodeLabelEnum.Variable => (await CreateOrUpdateVariableAsync(node, version)).Id,
                NodeLabelEnum.Parameter => new Random().Next(),
                NodeLabelEnum.External => new Random().Next(),
                _ => throw new NotImplementedException($"CreateOrUpdate not implemented for \"{node.Label}\"")
            };

            return nodeId;
        }

        public async Task MergeDuplicatesAsync(string appKey, string version)
        {
            var latestApp = (await _appRepository.GetAllLatestAsync(appKey, version)).FirstOrDefault();

            await MergeAppDuplicates(appKey, version);
            await MergeClassDuplicates(appKey, version);
            await MergeVariablesDuplicates(appKey, version);
            await MergeMethodDuplicates(appKey, version);
                
            await _appRepository.UpdateAddedOnFor(version, latestApp != null ? latestApp.Data.AddedOn + 1 : 0);
        }

        private async Task MergeAppDuplicates(string appKey, string version)
        {
            var newApps = await _appRepository.GetAllForAsync(appKey, version);
            var apps = await _appRepository.GetAllLatestAsync(appKey, version);

            if (apps.Any() && newApps.Any())
            {
                await _appRepository.AddRelationshipAsync(apps.First().Data.Id, newApps.First().Data.Id, "CHANGED_TO");
            }
        }

        private async Task MergeClassDuplicates(string appKey, string version)
        {
            var newClasses = await _classRepository.GetAllForAsync(appKey, version);
            var classes = await _classRepository.GetAllLatestAsync(appKey, version);

            if (classes.Any())
            {
                foreach (var newClass in newClasses)
                {
                    var class1 = classes.FirstOrDefault(x => x.Data.Usr == newClass.Data.Usr);
                    if (class1 != null)
                    {
                        await _classRepository.AddRelationshipAsync(class1.Data.Id, newClass.Data.Id, "CHANGED_TO");
                    }
                }
            }
        }

        private async Task MergeMethodDuplicates(string appKey, string version)
        {
            var newMethods = await _methodRepository.GetAllWithClassUsrForAsync(appKey, version);
            var methods = await _methodRepository.GetAllLatestWithClassUsrAsync(appKey, version);

            if (methods.Any())
            {
                foreach (var newMethod in newMethods)
                {
                    var outgoings = await _methodRepository.GetOutgoingRelationshipsAsync(newMethod.Method.Id);

                    if(newMethod.Method.Data.Name == "main")
                    {

                    }

                    var method = methods.FirstOrDefault(x => newMethod.Method.Data.Equals(x.Method.Data) && newMethod.ClassUsr == x.ClassUsr);
                    if (method.Method == null || !await NodesEqualsAsync(
                        outgoings.Select(x => (x.Data.End.Id, x.Data.End.Label, x.Data.Type)),
                        (await _methodRepository.GetOutgoingRelationshipsAsync(method.Method.Id)).Select(x => (x.Data.End.Id, x.Data.End.Label, x.Data.Type))))
                    {
                        var relatedMethod = method.Method ?? await _methodRepository.GetRelatedMethodAsync(newMethod.Method.Id, version);
                        if (relatedMethod != null)
                        {
                            await _methodRepository.AddRelationshipAsync(relatedMethod.Data.Id, newMethod.Method.Data.Id, "CHANGED_TO");
                        }
                    }
                    else
                    {
                        var incomings = await _methodRepository.GetIncomingRelationshipsAsync(newMethod.Method.Id);
                        foreach (var incoming in incomings)
                        {
                            await _methodRepository.AddRelationshipAsync(incoming.Data.Start.Id, method.Method.Id, incoming.Data.Type);
                        }

                        await _methodRepository.DeleteWithRelationshipAsync(newMethod.Method.Id);
                    }
                }
            }
        }

        private async Task<bool> NodesEqualsAsync(IEnumerable<(long Id, NodeLabelEnum Label, string IncomingRelationshipType)> nodes, IEnumerable<(long Id, NodeLabelEnum Label, string IncomingRelationshipType)> newNodes)
        {
            newNodes = newNodes.Where(x => x.IncomingRelationshipType != "CHANGED_TO");
            nodes = nodes.Where(x => x.IncomingRelationshipType != "CHANGED_TO");

            var apps = new Dictionary<long, Result<App>>();
            var classes = new Dictionary<long, Result<Class>>();
            var methods = new Dictionary<long, Result<Method>>();
            var variables = new Dictionary<long, Result<Variable>>();
            var externals = new Dictionary<long, Result<External>>();
            var parameters = new Dictionary<long, Result<Parameter>>();

            if (nodes.Count() != newNodes.Count())
            {
                return false;
            }

            async Task<bool> FindMatchAsync<TEntity>((long Id, NodeLabelEnum Label, string IncomingRelationshipType) newNode, INodeRepository<TEntity> nodeRepository, Dictionary<long, Result<TEntity>> cache) where TEntity : class, INode
            {
                if (!cache.TryGetValue(newNode.Id, out var node))
                {
                    node = await nodeRepository.GetByIdAndIncomingAsync(newNode.Id, newNode.IncomingRelationshipType);
                    cache.Add(newNode.Id, node);
                }
                var matchFound = false;

                foreach (var existingNode in nodes.Where(x => x.Label == newNode.Label).ToList())
                {
                    if (!cache.TryGetValue(existingNode.Id, out var foundExistingNode))
                    {
                        foundExistingNode = await nodeRepository.GetByIdAndIncomingAsync(existingNode.Id, existingNode.IncomingRelationshipType);
                        cache.Add(foundExistingNode.Id, foundExistingNode);
                    }

                    if (foundExistingNode.Data.Equals(node.Data))
                    {
                        matchFound = true;
                        break;
                    }
                }

                return matchFound;
            }

            foreach (var newNode in newNodes)
            {
                if (newNode.Label == NodeLabelEnum.App)
                {
                    if (!await FindMatchAsync(newNode, _appRepository, apps))
                    {
                        return false;
                    }
                }
                else if (newNode.Label == NodeLabelEnum.Class)
                {
                    if (!await FindMatchAsync(newNode, _classRepository, classes))
                    {
                        return false;
                    }
                }
                else if (newNode.Label == NodeLabelEnum.Method)
                {
                    if (!await FindMatchAsync(newNode, _methodRepository, methods))
                    {
                        return false;
                    }
                }
                else if (newNode.Label == NodeLabelEnum.Variable)
                {
                    if (!await FindMatchAsync(newNode, _variableRepository, variables))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private async Task MergeVariablesDuplicates(string appKey, string version)
        {
            var newVariables = await _variableRepository.GetAllForAsync(appKey, version);
            var variables = await _variableRepository.GetAllLatestAsync(appKey, version);

            if (variables.Any())
            {
                foreach (var newVariable in newVariables)
                {
                    var variable = variables.FirstOrDefault(x => newVariable.Data.Equals(x.Data));
                    if (variable == null)
                    {
                        var relatedVariable = variables.FirstOrDefault(x => newVariable.Data.Usr == x.Data.Usr);
                        if (relatedVariable != null)
                        {
                            await _methodRepository.AddRelationshipAsync(relatedVariable.Data.Id, newVariable.Data.Id, "CHANGED_TO");
                        }
                    }
                    else
                    {
                        var incomings = await _variableRepository.GetIncomingRelationshipsAsync(newVariable.Id);
                        foreach (var incoming in incomings)
                        {
                            await _variableRepository.AddRelationshipAsync(incoming.Data.Start.Id, variable.Id, incoming.Data.Type);
                            await _variableRepository.DeleteRelationshipAsync(incoming.Id);
                        }

                        var outgoings = await _variableRepository.GetOutgoingRelationshipsAsync(newVariable.Id);
                        foreach (var outgoing in outgoings)
                        {
                            await _variableRepository.AddRelationshipAsync(variable.Id, outgoing.Data.End.Id, outgoing.Data.Type);
                            await _variableRepository.DeleteRelationshipAsync(outgoing.Id);
                        }

                        await _variableRepository.DeleteWithRelationshipAsync(newVariable.Id);
                    }
                }
            }
        }

        public async Task<Result<App>> CreateOrUpdateAppAsync(NodeDto node, string version)
        {
            var app = await _appRepository.MergeAsync(node.Id, node.Properties, version);

            return app;
        }

        public async Task<Result<Class>> CreateOrUpdateClassAsync(NodeDto node, string version)
        {
            if (!node.Properties.TryGetValue("usr", out var usr))
            {
                throw new Exception("Required property usr is missing");
            }

            var class1 = await _classRepository.MergeAsync(node.Id, node.Properties, version);

            return class1;
        }

        public async Task<Result<Method>> CreateOrUpdateMethodAsync(NodeDto node, string version)
        {
            if (!node.Properties.TryGetValue("usr", out var usr))
            {
                throw new Exception("Required property usr is missing");
            }

            var method = await _methodRepository.MergeAsync(node.Id, node.Properties, version);

            return method;
        }

        public async Task<Result<Variable>> CreateOrUpdateVariableAsync(NodeDto node, string version)
        {
            if (!node.Properties.TryGetValue("usr", out var usr))
            {
                throw new Exception("Required property usr is missing");
            }

            var variable = await _variableRepository.MergeAsync(node.Id, node.Properties, version);

            return variable;
        }

        public Task RunQueryAsync(string query)
        {
            return _appRepository.RunQueryAsync(query);
        }

        public async Task AddRelationshipAsync(NodeDto from, NodeDto to, RelationshipDto relationship, string appKey, string version)
        {
            if (from.Label ==  NodeLabelEnum.App || to.Label == NodeLabelEnum.App)
            {

            }
            await CreateOrUpdateAsync(from, appKey, version);
            await CreateOrUpdateAsync(to, appKey, version);

            var propertiesString = "{ " + string.Join(", ", relationship.Properties.Select(x => x.Key + $": '{x.Value}'")) + " }";
            var t = $"match (a:{from.Label}), (c:{to.Label}) where a.id = {from.Id} and c.id = {to.Id} merge (a)-[r:{relationship.Type}{propertiesString}]->(c) return id(r)";
            await _appRepository.RunQueryAsync($"match (a:{from.Label}), (c:{to.Label}) where a.id = '{from.Id}' and c.id = '{to.Id}' merge (a)-[r:{relationship.Type}{propertiesString}]->(c) return id(r)");
        }

        private static void EnsureAppKey(NodeDto node, string appKey)
        {
            if (!node.Properties.ContainsKey("appKey"))
            {
                node.Properties.Add("appKey", appKey);
            }
        }
    }
}
