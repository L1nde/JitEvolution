using JitEvolution.Api.Dtos.IDE;
using JitEvolution.Services.Identity;
using JitEvolution.SignalR.Constants;
using JitEvolution.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JitEvolution.Api.Controllers.IDE
{
    [Route("ide/file-opened")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.DefaultScheme)]
    public class FileOpenedController : BaseController
    {
        private IHubContext<JitEvolutionHub> _hubContext;

        public FileOpenedController(IHubContext<JitEvolutionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> FileOpened([FromBody] FileOpenedDto dto)
        {
            await _hubContext.Clients.All.SendAsync(SignalRConstants.FileOpened, dto.ProjectId, dto.FileUri);

            return Ok();
        }
    }
}
