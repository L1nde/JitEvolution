using JitEvolution.Core.Models.Identity;

namespace JitEvolution.Core.Models
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }

        Guid CreatedById { get; set; }

        DateTime ChangedAt { get; set; }

        Guid ChangedById { get; set; }

        DateTime? DeletedAt { get; set; }

        Guid? DeletedById { get; set; }

        User CreatedBy { get; set; }

        User ChangedBy { get; set; }

        User DeletedBy { get; set; }
    }
}
