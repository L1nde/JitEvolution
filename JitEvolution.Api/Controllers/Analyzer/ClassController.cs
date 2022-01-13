using JitEvolution.Api.Dtos.Analyzer;
using JitEvolution.Core.Repositories.Analyzer;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Analyzer
{
    [Route("app/{appKey}/class")]
    public class ClassController : BaseController
    {
        private readonly IClassRepository _classRepository;

        public ClassController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ClassDto>> GetAll(long appKey)
        {
            return (await _classRepository.GetAll(appKey)).Select(x => new ClassDto
            {
                Id = x.Id,
                Code = x.Data.Code,
                Kind = x.Data.Kind,
                Name = x.Data.Name,
                Modifier = x.Data.Modifier,
                NumberOfLines = x.Data.NumberOfLines,
                Path = x.Data.Path,
                Usr = x.Data.Usr,
                VersionNUmber = x.Data.VersionNumber
            });
        }
    }
}
