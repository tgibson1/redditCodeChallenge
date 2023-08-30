using System;
using System.Diagnostics;
using code_challenge_reddit.Model;

namespace code_challenge_reddit.Services
{
	public interface IRedditFeedService
	{
		List<RedditPost> GetRedditPosts();
	}

    public class RedditFeedService : IRedditFeedService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string RedditBaseUrl = "https://www.reddit.com/r/";

        public RedditFeedService()
		{
		}

        public async Task<List<RedditPost>> GetRedditPosts(string subreddit, StatisticsTracker tracker)
        {
            // Reddit API request and response handling here
            var response = await _httpClient.GetStringAsync($"{RedditBaseUrl}{subreddit}/new.json?limit=100");
            var posts = ParsePosts(response);

            foreach (var post in posts)
            {
                tracker.UpdateStatistics(post);
            }
        }
    }
}

