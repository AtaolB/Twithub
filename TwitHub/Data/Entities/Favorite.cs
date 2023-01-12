namespace TwitHub.Data.Entities;

public class Favorite : BaseEntity
{
    public virtual Tweet Tweet { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}