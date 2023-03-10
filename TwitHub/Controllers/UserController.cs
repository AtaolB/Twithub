using Microsoft.AspNetCore.Identity;
using TwitHub.Data.Entities;
using TwitHub.Data.Repositories;
using TwitHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitHub.Models;


namespace TwitHub.Controllers
{
    public class UserController : Controller
    {
        private readonly ITweetHubRepository<Tweet> _TweetRepository;
        private readonly ITweetHubRepository<ReTweet> _ReTweetRepository;
        private readonly ITweetHubRepository<Favorite> _FavoriteRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public UserController(ILogger<HomeController> logger
            , ApplicationDbContext dbContext
            , UserManager<ApplicationUser> userManager
            , ITweetHubRepository<Tweet> TweetRepository
            , ITweetHubRepository<ReTweet> ReTweetRepository
            , ITweetHubRepository<Favorite> FavoriteRepository
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
            _TweetRepository = TweetRepository;
            _ReTweetRepository = ReTweetRepository;
            _FavoriteRepository = FavoriteRepository;
        }

        public async Task<IActionResult> RedirectUserPage()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userid = user.Id.ToString();
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Index(Guid? Id)
        {
            if (Id == null)
            {
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                var Tweet = await _dbContext.Tweets.OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == currentuser.Id).ToListAsync();
                var FollowCount = await _dbContext.FollowMaps.Where(x => x.SourceApplicationUser.Id == currentuser.Id).CountAsync();
                var FollowerCount = await _dbContext.FollowMaps.Where(x => x.TargetApplicationUser.Id == currentuser.Id).CountAsync();

                var Model = new UserPageViewModel()
                {
                    User = currentuser,
                    Tweets = Tweet,
                    FollowCount = FollowCount,
                    FollowerCount = FollowerCount
                };

                ViewBag.userid = currentuser.Id;
                ViewBag.Editing = Tweet;

                return View(Model);
            }
            else
            {
                var user = await _dbContext.Users.FindAsync(Id.ToString());
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                var Tweet = await _dbContext.Tweets.OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).ToListAsync();
                var FollowCount = await _dbContext.FollowMaps.Where(x => x.SourceApplicationUser.Id == user.Id).CountAsync();
                var FollowerCount = await _dbContext.FollowMaps.Where(x => x.TargetApplicationUser.Id == user.Id).CountAsync();

                var followcheck = await _dbContext.FollowMaps
                    .Where(x => x.SourceApplicationUser.Id == currentuser.Id).Where(x => x.TargetApplicationUser.Id == user.Id).FirstOrDefaultAsync();

                var Model = new UserPageViewModel()
                {
                    User = user,
                    Tweets = Tweet,
                    FollowCount = FollowCount,
                    FollowerCount = FollowerCount
                };

                ViewBag.userid = currentuser.Id;
                ViewBag.Editing = Tweet;
                ViewBag.CreateModel = new CreateTweetModel();
                ViewBag.FollowInfo = followcheck;

                return View(Model);
            }

        }

        public async Task<IActionResult> ReTweets(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var ReTweet = await _dbContext.ReTweets.OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).ToListAsync();
            var FollowCount = await _dbContext.FollowMaps.Where(x => x.SourceApplicationUser.Id == user.Id).CountAsync();
            var FollowerCount = await _dbContext.FollowMaps.Where(x => x.TargetApplicationUser.Id == user.Id).CountAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                ReTweets = ReTweet,
                FollowCount = FollowCount,
                FollowerCount = FollowerCount
            };


            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        public async Task<IActionResult> Favorites(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Favorite = await _dbContext.Favorites.OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).ToListAsync();
            var FollowCount = await _dbContext.FollowMaps.Where(x => x.SourceApplicationUser.Id == user.Id).CountAsync();
            var FollowerCount = await _dbContext.FollowMaps.Where(x => x.TargetApplicationUser.Id == user.Id).CountAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                Favorites = Favorite,
                FollowCount = FollowCount,
                FollowerCount = FollowerCount
            };


            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        public async Task<IActionResult> Follows(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Follow = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.SourceApplicationUser.Id == user.Id).ToListAsync();
            var FollowCount = await _dbContext.FollowMaps.Where(x => x.SourceApplicationUser.Id == user.Id).CountAsync();
            var FollowerCount = await _dbContext.FollowMaps.Where(x => x.TargetApplicationUser.Id == user.Id).CountAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                Follows = Follow,
                FollowCount = FollowCount,
                FollowerCount = FollowerCount
            };

            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        public async Task<IActionResult> Followers(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Followers = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.TargetApplicationUser.Id == user.Id).ToListAsync();
            var FollowCount = await _dbContext.FollowMaps.Where(x => x.SourceApplicationUser.Id == user.Id).CountAsync();
            var FollowerCount = await _dbContext.FollowMaps.Where(x => x.TargetApplicationUser.Id == user.Id).CountAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                Followers = Followers,
                FollowCount = FollowCount,
                FollowerCount = FollowerCount
            };

            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        public async Task<IActionResult> NewFollow(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            var followcheck = await _dbContext.FollowMaps
                .Where(x => x.SourceApplicationUser.Id == currentuser.Id).Where(x => x.TargetApplicationUser.Id == user.Id).FirstOrDefaultAsync();

            if (id.ToString() != currentuser.Id.ToString() && followcheck == null)
            {
                var newfollow = new FollowMap
                {
                    SourceApplicationUser = currentuser,
                    TargetApplicationUser = user

                };
                await _dbContext.FollowMaps.AddAsync(newfollow);
                await _dbContext.SaveChangesAsync();
            }

            ViewBag.userid = currentuser.Id;
            return RedirectToAction("Index", "User", new { id = id });
        }

        public async Task<IActionResult> UnFollow(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            var followcheck = await _dbContext.FollowMaps
                .Where(x => x.SourceApplicationUser.Id == currentuser.Id).Where(x => x.TargetApplicationUser.Id == user.Id).FirstOrDefaultAsync();

            if (followcheck != null)
            {
                _dbContext.FollowMaps.Remove(followcheck);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                ViewBag.userid = currentuser.Id;
                return RedirectToAction("Index", "User", new { id = id });
            }

            ViewBag.userid = currentuser.Id;
            return RedirectToAction("Index", "User", new { id = id });
        }

    }
}
