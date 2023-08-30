using code_challenge_reddit.Model;
using Microsoft.AspNetCore.Mvc;

namespace code_challenge_reddit.Controllers;

[ApiController]
[Route("[controller]")]
public class RedditStatisticsController : ControllerBase
{
    private readonly ILogger<RedditStatisticsController> _logger;

    public RedditStatisticsController(ILogger<RedditStatisticsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<RedditStatistic> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new RedditStatistic
        {
            
        })
        .ToArray();
    }
}
