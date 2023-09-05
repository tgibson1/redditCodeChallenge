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
        CacheUpdateService _cacheUpdateService { get; set; }
        public RedditStatisticService(CacheUpdateService cacheUpdateService) 
        {
            _cacheUpdateService = cacheUpdateService;
        }

        public List<PostVM> GetPostsWithMostUpvotes()
        {
            // Initialize variables to keep track of the child post with the most upvotes and the maximum upvote count.
            List<PostVM> mostUpvotes = new List<PostVM>();
            var _cache = _cacheUpdateService._cache;
            _cache.TryGetValue("posts", out RedditPost post);

            // Define a function to check and update the top child post.
            void CheckAndUpdateTopChild(Data childPost)
            {
                if (mostUpvotes.Any() && 
                    mostUpvotes.Count > 9 &&
                    childPost.ups > mostUpvotes?.Last()?.UpVotes)
                {
                    mostUpvotes.RemoveAt(mostUpvotes.IndexOf(mostUpvotes.Last()));
                    mostUpvotes.Add(new PostVM
                    {
                        ID = childPost.id,
                        Author = childPost.author,
                        Title = childPost.title,
                        UpVotes = childPost.ups
                    });
                }
                else if(childPost.ups > 0)
                {
                    mostUpvotes.Add(new PostVM
                    {
                        ID = childPost.id,
                        Author = childPost.author,
                        Title = childPost.title,
                        UpVotes = childPost.ups
                    });
                }

                mostUpvotes = mostUpvotes
                    .OrderByDescending(prop => prop.UpVotes)
                    ?.Take(10)
                    .ToList();
            }

            // Traverse the children of the provided RedditPost.
            foreach (var child in post.data.children)
            {
                if (child.kind == "t3") // Check if it's a post (t3 type).
                {
                    var childPost = child.data;
                    CheckAndUpdateTopChild(childPost);

                    // Traverse any potential replies within this child post.
                    if(childPost.children != null)
                        foreach (var reply in childPost.children)
                        {
                            if (reply.kind == "t3")
                            {
                                var replyPost = reply.data;
                                CheckAndUpdateTopChild(replyPost);
                            }
                        }
                }
            }

            // Return the ID of the child post with the most upvotes.
            return mostUpvotes;
        }

        public Dictionary<string, int> GetUsersWithMostPosts()
        {
            Dictionary<string, int> userPostCount = new Dictionary<string, int>();
            var _cache = _cacheUpdateService._cache;
            _cache.TryGetValue("posts", out RedditPost redditPost);

            // Check if the RedditPost object is not null
            if (redditPost != null)
            {
                // Traverse the children of the provided RedditPost.
                foreach (var child in redditPost.data.children)
                {
                    if (child.kind == "t3") // Check if it's a post (t3 type).
                    {
                        var childPost = child.data;
                        var author = childPost.author;

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

                        // Traverse any potential replies within this child post.
                        if (childPost.children != null)
                            foreach (var reply in childPost.children)
                            {
                                if (reply.kind == "t3")
                                {
                                    var replyPost = reply.data;
                                    var replyAuthor = replyPost.author;

                                    if (!string.IsNullOrWhiteSpace(replyAuthor))
                                    {
                                        if (userPostCount.ContainsKey(replyAuthor))
                                        {
                                            userPostCount[replyAuthor]++;
                                        }
                                        else
                                        {
                                            userPostCount[replyAuthor] = 1;
                                        }
                                    }
                                }
                            }
                    }
                }
            }

            // Order the users by post count
            return userPostCount.OrderByDescending(u => u.Value).Take(10).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

    }
}
