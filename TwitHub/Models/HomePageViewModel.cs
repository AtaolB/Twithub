using TwitHub.Data.Entities;

namespace TwitHub.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<TweetViewModel> TweetViewModel { get; set; }
        public Tweet Tweet { get; set; }
    }
}
 