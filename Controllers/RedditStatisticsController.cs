using code_challenge_reddit.Model;
using code_challenge_reddit.Services;
using Microsoft.AspNetCore.Mvc;

namespace code_challenge_reddit.Controllers;

[ApiController]
[Route("[controller]")]
public class RedditStatisticsController : ControllerBase
{
    private readonly ILogger<RedditStatisticsController> _logger;
    private IRedditStatisticService _redditStatisticService;

    public RedditStatisticsController(ILogger<RedditStatisticsController> logger,
                                        IRedditStatisticService redditStatisticService)
    {
        _logger = logger;
        _redditStatisticService = redditStatisticService;
    }

    [HttpGet(Name = "GetPostsWithMostUpvotes")]
    public async Task<ActionResult> GetPostsWithMostUpvotes()
    {
        var result = _redditStatisticService.GetPostsWithMostUpvotes();

        return Ok(result);
    }

    //[HttpGet(Name = "GetUsersWithMostPosts")]
    //public async Task<ActionResult> GetUsersWithMostPosts()
    //{
    //    var result = _redditStatisticService.GetUsersWithMostPosts();

    //    return Ok(result);
    //}
}
