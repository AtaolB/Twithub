namespace TwitHub.Data.Entities;

public class FollowMap : BaseEntity
{
    public virtual ApplicationUser SourceApplicationUser { get; set; }
    public virtual ApplicationUser TargetApplicationUser { get; set; }
}