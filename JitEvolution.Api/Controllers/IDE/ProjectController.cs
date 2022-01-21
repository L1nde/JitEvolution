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
        private readonly IProjectRepository _projectRepository;
        private readonly IMediator _mediator;
        private readonly CurrentUser currentUser;

        public ProjectController(IProjectService projectService, IMediator mediator, IProjectRepository projectRepository, CurrentUser currentUser)
        {
            _projectService = projectService;
            _mediator = mediator;
            _projectRepository = projectRepository;
            this.currentUser = currentUser;
        }

        [HttpPost]
        public async Task Create([FromForm] CreateProjectDto dto)
        {
            if (await _projectRepository.Queryable.AnyAsync(x => x.ProjectId == dto.ProjectId && x.UserId == currentUser.Id))
            {
                throw new Exception($"User already has project with id \"{dto.ProjectId}\"");
            }

            ValidateZipFile(dto.ProjectZip);
            
            await _projectService.CreateAsync(dto.ProjectId, dto.ProjectZip);

            await _mediator.Publish(new ProjectAdded());
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
