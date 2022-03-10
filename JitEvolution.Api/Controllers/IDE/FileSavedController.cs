using JitEvolution.Api.Dtos.IDE;
using JitEvolution.Core.Services.IDE;
using JitEvolution.Notifications;
using JitEvolution.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace JitEvolution.Api.Controllers.IDE
{
    [Route("ide/file-saved")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.DefaultScheme)]
    public class FileSavedController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IMediator _mediator;

        public FileSavedController(IProjectService projectService, IMediator mediator)
        {
            _projectService = projectService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> FileSaved([FromForm] FileSavedDto dto)
        {
            ValidateZipFile(dto.ProjectZip);

            await _projectService.CreateOrUpdateAsync(dto.ProjectId, dto.ProjectZip);

            await _mediator.Publish(new ProjectUpdated());

            return Ok();
        }

        private void ValidateZipFile(IFormFile file)
        {
            if (file.ContentType != MediaTypeNames.Application.Zip && file.ContentType != "application/x-zip-compressed")
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
