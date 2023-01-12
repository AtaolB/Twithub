using Microsoft.AspNetCore.Identity;

namespace TwitHub.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public virtual IList<Favorite> Favorites { get; set; }
    public virtual IList<ReTweet> ReTweets { get; set; }
    public virtual IList<Tweet> Tweets { get; set; }
}