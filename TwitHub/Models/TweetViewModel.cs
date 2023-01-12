using System.ComponentModel.DataAnnotations;
using TwitHub.Data.Entities;

namespace TwitHub.Models
{
    public class TweetViewModel
    {
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }

        [Required, StringLength(140, MinimumLength = 2)]
        public string Text { get; set; }

        public IList<Favorite> FavoriteUsers { get; set; }

        public IList<ReTweet> ReTweetUsers { get; set; }

        public int FavoriteCount { get; set; }

        public int ReTweetCount { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
