using Hangfire;
using Hangfire.Server;
using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Models.Queue;
using JitEvolution.Core.Repositories.Queue;
using JitEvolution.Core.Services.Analyzer;
using JitEvolution.Core.Services.Schedule;
using JitEvolution.Data.Constants.Identity;
using JitEvolution.Notifications;
using MediatR;

namespace JitEvolution.Hangfire.Services
{
    public class Scheduler : IScheduler, IJob, INotificationHandler<AnalyzeProject>
    {
        private readonly IQueueItemRepository _queueItemRepository;
        private readonly IAnalyzeService _analayzeService;
        private readonly CurrentUser _currentUser;
        private readonly IMediator _mediator;

        public Scheduler(IQueueItemRepository queueItemRepository, IAnalyzeService analayzeService, CurrentUser currentUser, IMediator mediator)
        {
            _queueItemRepository = queueItemRepository;
            _analayzeService = analayzeService;
            _currentUser = currentUser;
            _mediator = mediator;
        }

        public Task RunAsync(CancellationToken ct)
        {

            _currentUser.Id = UserConstants.SuperUserId;
            return ExecuteAsync(ct);
            
        }

        public Task RunAsync(Guid projectId, CancellationToken ct)
        {
            _currentUser.Id = UserConstants.SuperUserId;
            return ExecuteAsync(projectId, ct);
        }

        public async Task ExecuteAsync(CancellationToken ct)
        {
            var item = await _queueItemRepository.GetNextItemAsync();

            await ExecuteAsync(item, ct);
        }

        public async Task ExecuteAsync(Guid projectId, CancellationToken ct)
        {
            var item = await _queueItemRepository.GetNextItemForAsync(projectId);

            await ExecuteAsync(item, ct);
        }

        private async Task ExecuteAsync(QueueItem? item, CancellationToken ct)
        {
            if (item != null)
            {
                item.IsActive = true;

                await _queueItemRepository.SaveChangesAsync(ct);

                await _mediator.Publish(new ProjectUpdating(item.Project.UserId, item.Project.ProjectId));

                try
                {
                    await _analayzeService.AnalyzeAsync(item.Project.ProjectId, item.ProjectFilePath);

                    if (File.Exists(item.ProjectFilePath))
                    {
                        File.Delete(item.ProjectFilePath);
                    }

                    _queueItemRepository.Remove(item);

                    await _queueItemRepository.SaveChangesAsync(ct);
                }
                catch
                {
                    item.IsActive = false;

                    await _queueItemRepository.SaveChangesAsync(ct);
                }
                finally
                {
                    await _mediator.Publish(new ProjectUpdated(item.Project.UserId, item.Project.ProjectId));
                }
            }
        }

        public Task Handle(AnalyzeProject notification, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue<Scheduler>(x => x.RunAsync(notification.ProjectId, CancellationToken.None));

            return Task.CompletedTask;
        }
    }
}
