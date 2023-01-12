using Microsoft.AspNetCore.Identity;
using TwitHub.Data.Entities;
using TwitHub.Data.Repositories;
using TwitHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using TwitHub.Models;
using Microsoft.Data.SqlClient;
using NuGet.Packaging;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var Tweet = _TweetRepository.Listing().OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).Take(10);
            var ReTweet = _ReTweetRepository.Listing().OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).Take(10);
            var Favorites = _FavoriteRepository.Listing().OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).Take(10);
            var Follow = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.SourceApplicationUser.Id == user.Id).ToListAsync();
            var Follower = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.TargetApplicationUser.Id == user.Id).ToListAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                Tweets = Tweet,
                ReTweets = ReTweet,
                Favorites = Favorites,
                Follows = Follow,
                Followers = Follower
            };

            ViewBag.userid = user.Id;
            return View(Model);
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Tweet = _TweetRepository.Listing().OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).Take(10);

            var Model = new UserPageViewModel()
            {
                User = user,
                Tweets = Tweet
            };

            ViewBag.userid = currentuser.Id;

            return View(Model);
        }

        [Authorize]
        public async Task<IActionResult> ReTweets(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var ReTweet = _ReTweetRepository.Listing().OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).Take(10);

            var Model = new UserPageViewModel()
            {
                User = user,
                ReTweets = ReTweet
            };


            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        [Authorize]
        public async Task<IActionResult> Favorites(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Favorites = _FavoriteRepository.Listing().OrderByDescending(x => x.CreatedDate).Where(x => x.ApplicationUser.Id == user.Id).Take(10);

            var Model = new UserPageViewModel()
            {
                User = user,
                Favorites = Favorites
            };

            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        [Authorize]
        public async Task<IActionResult> Follows(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Follow = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.SourceApplicationUser.Id == currentuser.Id).ToListAsync();
            var Model = new UserPageViewModel()
            {
                User = user,
                Follows = Follow,
            };

            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        [Authorize]
        public async Task<IActionResult> Followers(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var Follow = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.TargetApplicationUser.Id == currentuser.Id).ToListAsync();
            var Model = new UserPageViewModel()
            {
                User = user,
                Follows = Follow,
            };

            ViewBag.userid = currentuser.Id;
            return View(Model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewFollow(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());

            var Follow = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.SourceApplicationUser.Id == user.Id).ToListAsync();
            var Follower = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.TargetApplicationUser.Id == user.Id).ToListAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                Follows = Follow,
                Followers = Follower
            };


            ViewBag.userid = user.Id;
            return View(Model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UnFollow(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id.ToString());

            var Follow = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.SourceApplicationUser.Id == user.Id).ToListAsync();
            var Follower = await _dbContext.FollowMaps.OrderByDescending(x => x.CreatedDate).Where(x => x.TargetApplicationUser.Id == user.Id).ToListAsync();

            var Model = new UserPageViewModel()
            {
                User = user,
                Follows = Follow,
                Followers = Follower
            };


            ViewBag.userid = user.Id;
            return View(Model);
        }

    }
}
