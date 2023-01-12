using Microsoft.AspNetCore.Mvc;
using TwitHub.Data.Entities;
using TwitHub.Models;
using Microsoft.AspNetCore.Identity;
using TwitHub.Data.Repositories;
using TwitHub.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TwitHub.Controllers
{
    public class TweetController : Controller
    {
        private readonly ITweetHubRepository<Tweet> _tweetRepository;
        private readonly ITweetHubRepository<Favorite> _favoriteRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public TweetController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ITweetHubRepository<Tweet> tweetRepository, ITweetHubRepository<Favorite> favoriteRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;

            _tweetRepository = tweetRepository;
            _favoriteRepository = favoriteRepository;
        }



        [Authorize]
        [HttpPost]

        public async Task<IActionResult> Create(CreateTweetModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var tweet = new Tweet
                {
                    Text = model.Text,
                    ApplicationUser = user,
                    CreatedBy = user.UserName
                };

                _tweetRepository.Insert(tweet);

                await _dbContext.SaveChangesAsync();

                ViewBag.Result = "Başarılı";

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Result = "Başarılı";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Favorite(Guid id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var tweet = _tweetRepository.GetContentById(id);

            var remember = user.Favorites.FirstOrDefault(x => x.Tweet.Id == tweet.Id);
            if (remember == null)
            {
                var favorite = new Favorite
                {
                    ApplicationUser = user,
                    Tweet = tweet
                };

                _dbContext.Favorites.Add(favorite);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.Favorites.Remove(remember);
                await _dbContext.SaveChangesAsync();
            }

            return Ok(new
            {
                IsDeleted = remember != null
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Retweet(Guid id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var tweet = _tweetRepository.GetContentById(id);

            if (user.Id != tweet.ApplicationUser.Id)
            {
                var remember = user.ReTweets.FirstOrDefault(x => x.Tweet.Id == tweet.Id);
                if (remember == null)
                {
                    var retweet = new ReTweet
                    {
                        ApplicationUser = user,
                        Tweet = tweet
                    };

                    _dbContext.ReTweets.Add(retweet);

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _dbContext.ReTweets.Remove(remember);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok(new
                {
                    IsDeleted = remember != null
                });
            }
            return null;
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            var tweet = await _dbContext.Tweets
                .FirstOrDefaultAsync(x => x.Id == id);
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.userid = currentuser.Id.ToString();

            return View(tweet);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {

            return View(_tweetRepository.GetContentById(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Tweet UpdateTweet)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var tweetalldetails = await _dbContext.Tweets.FirstOrDefaultAsync(x => x.Id == UpdateTweet.Id);

            if (user.Id == tweetalldetails.ApplicationUser.Id)
            {
                tweetalldetails.Text = UpdateTweet.Text;
                tweetalldetails.UpdatedDate = DateTime.Now;
                tweetalldetails.UpdatedBy = user.Id;

                _dbContext.Tweets.Update(tweetalldetails);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Tweet Düzenleme Başarısız";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tweet = _tweetRepository.GetContentById(id);
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            if (tweet.ApplicationUser.Id == currentuser.Id)
            {
                _dbContext.Tweets.Remove(tweet);
                var returnhome = await _dbContext.SaveChangesAsync();

                if (returnhome == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Tweet Düzenleme Başarısız";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Tweet Düzenleme Başarısız";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}