using JitEvolution.Api.Dtos.Analyzer;
using JitEvolution.Core.Repositories.Analyzer;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Analyzer
{
    [Route("app/{appId}/class/{classId}/method")]
    public class MethodController : BaseController
    {
        private readonly IMethodRepository _methodRepository;

        public MethodController(IMethodRepository methodRepository)
        {
            _methodRepository = methodRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<MethodDto>> GetAll(long appId, long classId)
        {
            return (await _methodRepository.GetAllAsync(appId, classId)).Select(x => new MethodDto
            {
                Id = x.Id,
                Code = x.Data.Code,
                Kind = x.Data.Kind,
                Name = x.Data.Name,
                CyclomaticComplexity = x.Data.CyclomaticComplexity,
                EndLine = x.Data.EndLine,
                IsConstructor = x.Data.IsConstructor,
                IsGetter = x.Data.IsGetter,
                IsSetter = x.Data.IsSetter,
                MaxNestingDepth = x.Data.MaxNestingDepth,
                NumberOfAccessedVariables = x.Data.NumberOfAccessedVariables,
                NumberOfCalledMethods = x.Data.NumberOfCalledMethods,
                NumberOfCallers = x.Data.NumberOfCallers,
                NumberOfInstructors = x.Data.NumberOfInstructors,
                StartLine = x.Data.StartLine,
                Type = x.Data.Type,
                Usr = x.Data.Usr,
                VersionNumber = x.Data.VersionNumber,
            });
        }
    }
}
