using JitEvolution.Api.Dtos.Analyzer;
using JitEvolution.Core.Repositories.Analyzer.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Analyzer
{
    [Route("app")]
    public class AppController : BaseController
    {
        private readonly IAppRepository _appRepository;
        private readonly IClassRepository _classRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly IVariableRepository _variableRepository;

        public AppController(IAppRepository appRepository, IClassRepository classRepository, IMethodRepository methodRepository, IVariableRepository variableRepository)
        {
            _appRepository = appRepository;
            _classRepository = classRepository;
            _methodRepository = methodRepository;
            _variableRepository = variableRepository;
        }

        [HttpGet("version-number")]
        public Task<IEnumerable<int>> GetAppVersions(string projectId)
        {
            return _appRepository.GetAppVersionNumbersAsync(projectId);
        }

        [HttpGet()]
        public async Task<AppDetailDto> Get(string projectId, int? versionNumber)
        {
            var apps = await _appRepository.GetResultAsync(projectId, versionNumber);

            var app = apps.FirstOrDefault();

            if (app == null)
            {
                return null;
            }

            return new AppDetailDto
            {
                Id = app.Id,
                AppKey = app.AppKey,
                Name = app.Name,
                VersionNumber = app.VersionNumber,
                AddedOn = app.AddedOn,
                Classes = app.Classes.OrderBy(x => x.Name).Select(x => new ClassDetailDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Kind = x.Kind,
                    Name = x.Name,
                    Modifier = x.Modifier,
                    NumberOfLines = x.NumberOfLines,
                    Path = x.Path,
                    Usr = x.Usr,
                    VersionNUmber = x.VersionNumber,
                    AddedOn = x.AddedOn,
                    Methods = x.Methods.OrderBy(x => x.Kind).ThenBy(x => x.Name).Select(y => new MethodDetailDto
                    {
                        Id = y.Id,
                        Code = y.Code,
                        Kind = y.Kind,
                        Name = y.Name,
                        CyclomaticComplexity = y.CyclomaticComplexity,
                        EndLine = y.EndLine,
                        IsConstructor = y.IsConstructor,
                        IsGetter = y.IsGetter,
                        IsSetter = y.IsSetter,
                        MaxNestingDepth = y.MaxNestingDepth,
                        NumberOfAccessedVariables = y.NumberOfAccessedVariables,
                        NumberOfCalledMethods = y.NumberOfCalledMethods,
                        NumberOfCallers = y.NumberOfCallers,
                        NumberOfInstructors = y.NumberOfInstructors,
                        StartLine = y.StartLine,
                        Type = y.Type,
                        Usr = y.Usr,
                        VersionNumber = y.VersionNumber,
                        AddedOn = y.AddedOn,
                        Calls = y.Calls,
                        Uses = y.Uses,
                        Modifier = y.Modifier
                    }),
                    Variables = x.Variables.OrderBy(x => x.Name).Select(y => new VariableDetailDto
                    {
                        Id = y.Id,
                        Code = y.Code,
                        Kind = y.Kind,
                        Name = y.Name,
                        EndLine = y.EndLine,
                        StartLine = y.StartLine,
                        Type = y.Type,
                        Usr = y.Usr,
                        VersionNumber = y.VersionNumber,
                        AddedOn = y.AddedOn,
                    }),
                })
            };
        }
    }
}
