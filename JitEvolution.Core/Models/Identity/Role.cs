using Microsoft.AspNetCore.Identity;

namespace JitEvolution.Core.Models.Identity
{
    public class Role : IdentityRole<Guid>, IBaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime ChangedAt { get; set; }

        public Guid ChangedById { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedById { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual User ChangedBy { get; set; }

        public virtual User DeletedBy { get; set; }
    }
}
