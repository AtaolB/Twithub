using TwitHub.Data.Entities;

namespace TwitHub.Models
{
    public class UserPageTweetListingModel
    {
        public IEnumerable<Tweet> tweet { get; set; }
    }
}
