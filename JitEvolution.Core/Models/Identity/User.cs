using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace JitEvolution.Core.Models.Identity
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public string? AccessKey { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime ChangedAt { get; set; }

        public Guid ChangedById { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedById { get; set; }

        [NotMapped]
        public virtual User CreatedBy { get; set; }

        [NotMapped]
        public virtual User ChangedBy { get; set; }

        [NotMapped]
        public virtual User DeletedBy { get; set; }
    }
}
