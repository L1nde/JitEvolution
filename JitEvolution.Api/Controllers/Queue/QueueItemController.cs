using JitEvolution.Api.Dtos.Queue;
using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Repositories.Queue;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Queue
{
    [Route("queue-item")]
    public class QueueItemController : BaseController
    {
        private readonly IQueueItemRepository _queueItemRepository;
        private readonly CurrentUser _currentUser;

        public QueueItemController(IQueueItemRepository queueItemRepository, CurrentUser currentUser)
        {
            _queueItemRepository = queueItemRepository;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<List<QueueItemDto>> List()
        {
            var items = await _queueItemRepository.ListAsync(_queueItemRepository.Queryable.Where(x => x.Project.UserId == _currentUser.Id));

            return items.OrderBy(x => x.CreatedAt).Select(x => new QueueItemDto
            {
                ProjectId = x.Project.ProjectId,
                IsActive = x.IsActive,
                ChangedAtUtc = x.ChangedAt
            }).ToList();
        }
    }
}
