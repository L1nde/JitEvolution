using JitEvolution.Api.Dtos.Analyzer;
using JitEvolution.Core.Enums.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Models.Analyzer.GraphifyEvolution;
using JitEvolution.Core.Services.Analyzer.GraphifyEvolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace JitEvolution.Api.Controllers.Analyzer.GraphifyEvolution
{
    [Route("analyzer-result")]
    public class AnalyzerResultController : BaseController
    {
        private readonly INodeService _nodeService;

        public AnalyzerResultController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost("node")]
        [AllowAnonymous]
        public async Task<AnalyzerResponseDto> Node([FromBody] ResultDto<NodeDto> dto)
        {
            var id = await _nodeService.CreateOrUpdateAsync(dto.Data, dto.AppKey, dto.Version);

            return new AnalyzerResponseDto(id);
        }

        [HttpPost("relationship")]
        [AllowAnonymous]
        public Task Relationship([FromBody] ResultDto<RelationshipAddDto> dto)
        {
            return _nodeService.AddRelationshipAsync(dto.Data.From, dto.Data.To, dto.Data.Relationship, dto.AppKey, dto.Version);
        }

        [HttpPost("query")]
        [AllowAnonymous]
        public async Task<AnalyzerResponseDto> Post(QueryDto dto)
        {
            foreach (var statementDto in dto.Statements)
            {
                await _nodeService.RunQueryAsync(Regex.Replace(statementDto.Statement, "id[(](.+?)[)]", "$1.id"));
            }

            return new AnalyzerResponseDto(new Random().Next());
        }

        [HttpPost("merge-duplicates")]
        [AllowAnonymous]
        public async Task<AnalyzerResponseDto> MergeDuplicates(MergeDuplicatesDto dto)
        {
            await _nodeService.MergeDuplicatesAsync(dto.AppKey, dto.Version);

            return new AnalyzerResponseDto(new Random().Next());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<AnalyzerResponseDto> Post()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var t = await reader.ReadToEndAsync();


            }

            //Debug.WriteLine(string.Join(", ", dto.Statements.Select(x => x.Statement))+ "\n");
            return new AnalyzerResponseDto(new Random().Next());
        }
    }
}
