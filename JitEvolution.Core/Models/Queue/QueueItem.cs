using JitEvolution.Core.Models.IDE;

namespace JitEvolution.Core.Models.Queue
{
    public class QueueItem : BaseEntity
    {
        public Guid ProjectId { get; set; }

        public bool IsActive { get; set; } = false;

        public string ProjectFilePath { get; set; }


        public virtual Project Project { get; set; }
    }
}
