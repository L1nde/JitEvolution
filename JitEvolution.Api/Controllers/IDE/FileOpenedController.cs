using JitEvolution.Api.Dtos.IDE;
using JitEvolution.Notifications;
using JitEvolution.Services.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JitEvolution.Api.Controllers.IDE
{
    [Route("ide/file-opened")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.DefaultScheme)]
    public class FileOpenedController : BaseController
    {
        private readonly IMediator _mediator;

        public FileOpenedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> FileOpened([FromBody] FileOpenedDto dto)
        {

            await _mediator.Publish(new FileOpened(Guid.Parse(User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value), dto.ProjectId, dto.FileUri));

            return Ok();
        }
    }
}
