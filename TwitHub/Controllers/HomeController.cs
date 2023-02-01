using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TwitHub.Data.Entities;
using TwitHub.Data;
using TwitHub.Models;
using TwitHub.Data.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TwitHub.Controllers
{
    public class HomeController : Controller
    {
        private ITweetHubRepository<Tweet> _GenericRepository;
        private readonly ITweetHubRepository<ReTweet> _ReTweetRepository;
        private readonly ITweetHubRepository<Favorite> _FavoriteRepository;
        private readonly ITweetHubRepository<ApplicationUser> _UserRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger
            , ApplicationDbContext dbContext
            , UserManager<ApplicationUser> userManager
            , ITweetHubRepository<Tweet> genericRepository
            , ITweetHubRepository<ReTweet> ReTweetRepository
            , ITweetHubRepository<Favorite> FavoriteRepository
            , ITweetHubRepository<ApplicationUser> UserRepository
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
            _GenericRepository = genericRepository;
            _ReTweetRepository = ReTweetRepository;
            _FavoriteRepository = FavoriteRepository;
            _UserRepository = UserRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var useridstring = user.Id.ToString();
            var createmodel = new CreateTweetModel();
            ViewBag.CreateModel = createmodel;

            var TweetList = _GenericRepository.Listing()
                .OrderByDescending(x => x.CreatedDate)
                .Take(100);

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.userid = currentuser.Id;
            return View(TweetList);
        }

        //TO DO
        public async Task<IActionResult> TweetInfinityListing(int page)
        {
            var TweetList = _GenericRepository.Listing()
                         .OrderByDescending(x => x.CreatedDate)
                         .Select(
                 t => new TweetViewModel
                 {
                     Id = t.Id,
                     User = t.ApplicationUser,
                     Text = t.Text,
                     DatePosted = t.CreatedDate.Value,
                     FavoriteUsers = t.Favorites,
                     ReTweetUsers = t.ReTweets,
                     FavoriteCount = t.Favorites.Count(),
                     ReTweetCount = t.ReTweets.Count(),
                 })
             .Skip(10 * (page - 1))
             .Take(10);

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.userid = currentuser.Id;
            return this.PartialView("_TweetPartial", TweetList);
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {

            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "cosmus",
                Email = "coskun.bagriacik@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var appUser2 = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "ataolb",
                Email = "ataolb@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var appUser3 = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "obb",
                Email = "obb@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            await _userManager.CreateAsync(appUser, "A.123456");
            await _userManager.CreateAsync(appUser2, "A.123456");
            await _userManager.CreateAsync(appUser3, "A.123456");

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var t1 = new Tweet { Text = "Tweet1", ApplicationUser = appUser };
            var t2 = new Tweet { Text = "Tweet2", ApplicationUser = appUser2 };
            var t3 = _dbContext.Tweets.FirstOrDefault();

            var test = await _dbContext.Tweets.AddAsync(t1);
            var test1 = await _dbContext.Tweets.AddAsync(t2);

            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = currentuser, TargetApplicationUser = appUser2 });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = currentuser, TargetApplicationUser = appUser });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = currentuser, TargetApplicationUser = appUser3 });

            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser2, TargetApplicationUser = currentuser });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser, TargetApplicationUser = currentuser });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser3, TargetApplicationUser = currentuser });

            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser, TargetApplicationUser = appUser2 });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser, TargetApplicationUser = appUser3 });

            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser2, TargetApplicationUser = appUser });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser2, TargetApplicationUser = appUser3 });

            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser3, TargetApplicationUser = appUser });
            await _dbContext.FollowMaps.AddAsync(new FollowMap { SourceApplicationUser = appUser3, TargetApplicationUser = appUser2 });


            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}