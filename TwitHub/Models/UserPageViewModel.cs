using TwitHub.Data.Entities;

namespace TwitHub.Models
{
    public class UserPageViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Tweet> Tweets { get; set; }
        public IEnumerable<ReTweet> ReTweets { get; set; }
        public IEnumerable<Favorite> Favorites { get; set; }
        public IEnumerable<FollowMap> Follows { get; set; }
        public IEnumerable<FollowMap> Followers { get; set; }
    }
}
