using code_challenge_reddit.Model;
using Microsoft.Extensions.Caching.Memory;

namespace code_challenge_reddit.Services
{
    public interface IRedditDataProvider
    {
        RedditPost GetRedditPost();
    }

    public class RedditDataProvider : IRedditDataProvider
    {
        private readonly IMemoryCache _cache;

        public RedditDataProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public RedditPost GetRedditPost()
        {
            _cache.TryGetValue("posts", out RedditPost post);
            return post;
        }
    }
}
