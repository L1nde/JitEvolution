using JitEvolution.Api.Dtos.IDE;
using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Repositories.IDE;
using JitEvolution.Core.Services.IDE;
using JitEvolution.Notifications;
using JitEvolution.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace JitEvolution.Api.Controllers.IDE
{
    [Route("ide/project")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.DefaultScheme)]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService, IMediator mediator, IProjectRepository projectRepository, CurrentUser currentUser)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProjectDto dto)
        {
            ValidateZipFile(dto.ProjectZip);
            
            await _projectService.CreateOrUpdateAsync(dto.ProjectId, dto.ProjectZip);

            return Ok();
        }

        private void ValidateZipFile(IFormFile file)
        {
            if (file.ContentType != MediaTypeNames.Application.Zip && file.ContentType != "application/x-zip-compressed" )
            {
                throw new Exception("Invalid content-type");
            }
            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                throw new Exception("Invalid file name");
            }
        }
    }
}
