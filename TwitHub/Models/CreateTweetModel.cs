using System.ComponentModel.DataAnnotations;

namespace TwitHub.Models
{
    public class CreateTweetModel
    {
        [Required, StringLength(140, MinimumLength = 2)]
        public string Text { get; set; }
    }
}
