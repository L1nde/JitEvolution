using JitEvolution.Core.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JitEvolution.Core.Models.IDE
{
    [Table("Project")]
    public class Project : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string ProjectId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
    }
}
