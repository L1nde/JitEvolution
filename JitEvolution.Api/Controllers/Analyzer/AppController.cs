using JitEvolution.Api.Dtos.Analyzer;
using JitEvolution.Core.Repositories.Analyzer;
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

        //[HttpGet]
        //public async Task<IEnumerable<AppDto>> GetAll()
        //{
        //    return (await _appRepository.GetAll()).Select(x => new AppDto
        //    {
        //        Id = x.Id,
        //        AppKey = x.Data.AppKey,
        //        Name = x.Data.Name,
        //        VersionNumber = x.Data.VersionNumber
        //    });
        //}

        [HttpGet()]
        public async Task<AppDetailDto> Get(string projectId)
        {
            var result = await _appRepository.GetAsync(projectId);

            return new AppDetailDto
            {
                Id = result.Id,
                AppKey = result.Data.AppKey,
                Name = result.Data.Name,
                VersionNumber = result.Data.VersionNumber,
                Classes = await Task.WhenAll((await _classRepository.GetAllAsync(result.Id)).Select(async x => new ClassDetailDto
                {
                    Id = x.Id,
                    Code = x.Data.Code,
                    Kind = x.Data.Kind,
                    Name = x.Data.Name,
                    Modifier = x.Data.Modifier,
                    NumberOfLines = x.Data.NumberOfLines,
                    Path = x.Data.Path,
                    Usr = x.Data.Usr,
                    VersionNUmber = x.Data.VersionNumber,
                    Methods = (await _methodRepository.GetAllAsync(result.Id, x.Id, "method.start_line <> 0 and method.end_line <> 0")).Select(y => new MethodDetailDto
                    {
                        Id = y.Id,
                        Code = y.Data.Code,
                        Kind = y.Data.Kind,
                        Name = y.Data.Name,
                        CyclomaticComplexity = y.Data.CyclomaticComplexity,
                        EndLine = y.Data.EndLine,
                        IsConstructor = y.Data.IsConstructor,
                        IsGetter = y.Data.IsGetter,
                        IsSetter = y.Data.IsSetter,
                        MaxNestingDepth = y.Data.MaxNestingDepth,
                        NumberOfAccessedVariables = y.Data.NumberOfAccessedVariables,
                        NumberOfCalledMethods = y.Data.NumberOfCalledMethods,
                        NumberOfCallers = y.Data.NumberOfCallers,
                        NumberOfInstructors = y.Data.NumberOfInstructors,
                        StartLine = y.Data.StartLine,
                        Type = y.Data.Type,
                        Usr = y.Data.Usr,
                        VersionNumber = y.Data.VersionNumber,
                    }),
                    MethodsCalls = (await _methodRepository.GetAllRelationshipsAsync(result.Id, x.Id, "method.start_line <> 0 and method.end_line <> 0")).Select(y => new Core.Models.Analyzer.RelationshipDto
                    {
                        Type = y.Data.Type,
                        Start = y.Data.Start,
                        End = y.Data.End
                    }),
                    Variables = (await _variableRepository.GetAllAsync(result.Id, x.Id)).Select(y => new VariableDetailDto
                    {
                        Id = y.Id,
                        Code = y.Data.Code,
                        Kind = y.Data.Kind,
                        Name = y.Data.Name,
                        EndLine = y.Data.EndLine,
                        StartLine = y.Data.StartLine,
                        Type = y.Data.Type,
                        Usr = y.Data.Usr,
                        VersionNumber = y.Data.VersionNumber
                    })
                }))
            };
        }
    }
}
