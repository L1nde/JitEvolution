using JitEvolution.Api.Dtos.IDE;
using JitEvolution.Api.Dtos.Identity;
using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Repositories.IDE;
using JitEvolution.Core.Services.Identity;
using JitEvolution.Data.Constants.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Identity
{
    [Route("user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly CurrentUser _currentUser;
        private readonly IProjectRepository _projectRepository;

        public UserController(IUserService userService, CurrentUser currentUser, IProjectRepository projectRepository)
        {
            _userService = userService;
            _currentUser = currentUser;
            _projectRepository = projectRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<UserDto> Register(RegisterDto dto)
        {
            using (_currentUser.Overrider(UserConstants.AnonymousUserId))
            {
                var user = await _userService.CreateAsync(dto.Username, dto.Password, dto.Email);

                return new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    AccessKey = user.AccessKey
                };
            }
        }

        [HttpGet("{userId}/project")]
        public async Task<IEnumerable<ProjectDto>> GetUserProjects(Guid userId)
        {
            return (await _projectRepository.ListAsync(_projectRepository.Queryable.Where(x => x.UserId == userId))).Select(x => new ProjectDto
            {
                Id = x.Id,
                UserId = x.UserId,
                ProjectId = x.ProjectId
            });
        }

    }
}
