using code_challenge_reddit.Model;
using Microsoft.Extensions.Caching.Memory;

namespace code_challenge_reddit.Services
{
    public interface IRedditStatisticService
    {
        List<PostVM> GetPostsWithMostUpvotes();
        Dictionary<string, int> GetUsersWithMostPosts();
    }
    public class RedditStatisticService : IRedditStatisticService
    {
        private readonly IRedditDataProvider _dataProvider;
        private readonly IRedditStatisticsCalculator _calculator;

        public RedditStatisticService(IRedditDataProvider dataProvider, IRedditStatisticsCalculator calculator)
        {
            _dataProvider = dataProvider;
            _calculator = calculator;
        }

        public List<PostVM> GetPostsWithMostUpvotes()
        {
            RedditPost post = _dataProvider.GetRedditPost();
            return _calculator.GetPostsWithMostUpvotes(post);
        }

        public Dictionary<string, int> GetUsersWithMostPosts()
        {
            RedditPost post = _dataProvider.GetRedditPost();
            return _calculator.GetUsersWithMostPosts(post);
        }

    }
}
