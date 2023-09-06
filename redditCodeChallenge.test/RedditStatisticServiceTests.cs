using code_challenge_reddit.Model;
using code_challenge_reddit.Services;
using Moq;

namespace code_challenge_reddit.test
{
    public class RedditStatisticServiceTests
    {
        [Fact]
        public void GetPostsWithMostUpvotes_ReturnsTop10Posts()
        {
            // Arrange
            var redditDataProviderMock = new Mock<IRedditDataProvider>();
            var redditStatisticsCalculatorMock = new Mock<IRedditStatisticsCalculator>();
            var redditStatisticService = new RedditStatisticService(redditDataProviderMock.Object, redditStatisticsCalculatorMock.Object);

            // Mock the RedditPost object from data provider
            var redditPost = new RedditPost
            {
                kind = "dummy",
                data = new Data
                {
                    children = new List<Child>
                    {
                        new Child
                        {
                            kind = "t3",
                            data = new Data
                            {
                                id = "1",
                                author = "user1",
                                title = "Post 1",
                                ups = 100
                            }
                        },
                    }
                }
            };

            // Mock the data provider to return the RedditPost
            redditDataProviderMock.Setup(dp => dp.GetRedditPost()).Returns(redditPost);

            // Mock the statistics calculator to return the top posts
            redditStatisticsCalculatorMock.Setup(calc => calc.GetPostsWithMostUpvotes(redditPost))
                .Returns(new List<PostVM>
                {
                    new PostVM
                    {
                        ID = "1",
                        Author = "user1",
                        Title = "Post 1",
                        UpVotes = 100
                    },
                });

            // Act
            var topPosts = redditStatisticService.GetPostsWithMostUpvotes();

            // Assert
            Assert.NotNull(topPosts);
            Assert.Equal(1, topPosts.Count);
        }

        [Fact]
        public void GetUsersWithMostPosts_ReturnsTop10Users()
        {
            // Arrange
            var redditDataProviderMock = new Mock<IRedditDataProvider>();
            var redditStatisticsCalculatorMock = new Mock<IRedditStatisticsCalculator>();
            var redditStatisticService = new RedditStatisticService(redditDataProviderMock.Object, redditStatisticsCalculatorMock.Object);

            // Mock the RedditPost object from data provider
            var redditPost = new RedditPost
            {
                kind = "dummy",
                data = new Data
                {
                    children = new List<Child>
                    {
                        new Child
                        {
                            kind = "t3",
                            data = new Data
                            {
                                author = "user1"
                            }
                        },
                    }
                }
            };

            // Mock the data provider to return the RedditPost
            redditDataProviderMock.Setup(dp => dp.GetRedditPost()).Returns(redditPost);

            // Mock the statistics calculator to return the top users
            redditStatisticsCalculatorMock.Setup(calc => calc.GetUsersWithMostPosts(redditPost))
                .Returns(new Dictionary<string, int>
                {
                    { "user1", 5 },
                });

            // Act
            var topUsers = redditStatisticService.GetUsersWithMostPosts();

            // Assert
            Assert.NotNull(topUsers);
            Assert.Equal(1, topUsers.Count);
        }
    }
}
