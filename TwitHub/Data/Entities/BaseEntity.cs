using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitHub.Data.Entities;

public class BaseEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public virtual bool IsActive { get; set; } = true;

    public virtual DateTime? CreatedDate { get; set; } = DateTime.Now;
    public virtual string? CreatedBy { get; set; }

    public virtual DateTime? UpdatedDate { get; set; }
    public virtual string? UpdatedBy { get; set; }

    public virtual bool IsDeleted { get; set; } = false;
    public virtual DateTime? DeletedDate { get; set; }
    public virtual string? DeletedBy { get; set; }
}
