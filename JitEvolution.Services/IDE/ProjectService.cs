using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Config;
using JitEvolution.Core.Models.Queue;
using JitEvolution.Core.Repositories.IDE;
using JitEvolution.Core.Repositories.Queue;
using JitEvolution.Core.Services.IDE;
using JitEvolution.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JitEvolution.Services.IDE
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly CurrentUser _currentUser;
        private readonly Configuration _config;
        private readonly IFileService _fileService;
        private readonly IQueueItemRepository _queueItemRepository;
        private readonly IMediator _mediator;

        public ProjectService(IProjectRepository projectRepository, CurrentUser currentUser, IOptions<Configuration> config, IFileService fileService, IQueueItemRepository queueItemRepository, IMediator mediator)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _config = config.Value;
            _fileService = fileService;
            _queueItemRepository = queueItemRepository;
            _mediator = mediator;
        }

        public async Task CreateOrUpdateAsync(string projectId, IFormFile projectZipFile)
        {
            var project = await _projectRepository.GetByProjectIdAsync(projectId);

            if (project == null)
            {
                project = new Core.Models.IDE.Project
                {
                    UserId = _currentUser.Id,
                    ProjectId = projectId
                };

                await _projectRepository.AddAsync(project);

                await _projectRepository.SaveChangesAsync();

                await _mediator.Publish(new ProjectAdded(project.UserId));
            }

            var filePath = await _fileService.SaveAsync(projectZipFile);

            await _queueItemRepository.AddAsync(new QueueItem
            {
                ProjectId = project.Id,
                ProjectFilePath = filePath,
                IsActive = false
            });

            await _queueItemRepository.SaveChangesAsync();

            await _mediator.Publish(new AnalyzeProject(project.Id));
        }
    }
}
