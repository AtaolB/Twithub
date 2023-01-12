using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitHub.Data.Entities;

namespace TwitHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<FollowMap> FollowMaps { get; set; }
        public DbSet<ReTweet> ReTweets { get; set; }
        public DbSet<Tweet> Tweets { get; set; }

    }
}