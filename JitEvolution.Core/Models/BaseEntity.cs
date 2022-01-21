using JitEvolution.Core.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JitEvolution.Core.Models
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid CreatedById { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [Required]
        public Guid ChangedById { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedById { get; set; }

        [NotMapped]
        public virtual User CreatedBy { get; set; }

        [NotMapped]
        public virtual User ChangedBy { get; set; }

        [NotMapped]
        public virtual User? DeletedBy { get; set; }
    }
}
