using JitEvolution.Api.Dtos.IDE;
using JitEvolution.Services.Identity;
using JitEvolution.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JitEvolution.Api.Controllers.IDE
{
    [Route("ide/file-saved")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.DefaultScheme)]
    public class FileSavedController : BaseController
    {
        private IHubContext<JitEvolutionHub> _hubContext;

        public FileSavedController(IHubContext<JitEvolutionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> FileSaved([FromBody] FileSavedDto dto)
        {
            return Ok();
        }
    }
}
