using System.ComponentModel.DataAnnotations;

namespace TwitHub.Data.Entities;

public class Tweet : BaseEntity
{
    [Required, StringLength(140, MinimumLength = 2)]
    public string Text { get; set; }

    public virtual IList<Favorite> Favorites { get; set; }
    public virtual IList<ReTweet> ReTweets { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}