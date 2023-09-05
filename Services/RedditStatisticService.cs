using code_challenge_reddit.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace code_challenge_reddit.Services
{
    public class RedditStatisticService
    {
        CacheUpdateService _cacheUpdateService { get; set; }
        public RedditStatisticService(CacheUpdateService cacheUpdateService) 
        {
            _cacheUpdateService = cacheUpdateService;
        }
        public List<Child> GetPostsWithMostUpvotes()
        {
            List<Child> mostUpVoted;
            var _cache = _cacheUpdateService._cache;
            if (_cache.TryGetValue("redirects", out RedditPost posts))
            {
                mostUpVoted = posts.data.children?.OrderByDescending(p => p.data.ups).Take(10).ToList();
            }
            // Implement your logic to get posts with the most upvotes
            // You can use LINQ to order the posts by upvotes and take the top N
            return null;
        }

        public Dictionary<string, int> GetUsersWithMostPosts(List<Child> posts)
        {
            // Implement your logic to get users with the most posts
            // You can use a dictionary to track the post count for each user
            var userPostCount = new Dictionary<string, int>();

            foreach (var post in posts)
            {
                var author = post.data.author;
                if (!string.IsNullOrWhiteSpace(author))
                {
                    if (userPostCount.ContainsKey(author))
                    {
                        userPostCount[author]++;
                    }
                    else
                    {
                        userPostCount[author] = 1;
                    }
                }
            }

            // Order the users by post count
            return userPostCount.OrderByDescending(u => u.Value).Take(10).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
