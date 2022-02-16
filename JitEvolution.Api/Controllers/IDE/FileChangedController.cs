using JitEvolution.Api.Dtos.IDE;
using JitEvolution.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.IDE
{
    [Route("ide/file-changed")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSchemeOptions.DefaultScheme)]
    public class FileChangedController
    {
        [HttpPost]
        public Task FileChanged(FileChangedDto dto)
        {
            return Task.CompletedTask;
        }
    }
}
