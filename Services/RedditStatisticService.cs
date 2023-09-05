﻿using code_challenge_reddit.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System.Linq;

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
        //public List<Child> GetPostsWithMostUpvotes()
        //{
        //    List<Child> mostUpVoted = null;
        //    var _cache = _cacheUpdateService._cache;
        //    if (_cache.TryGetValue("posts", out RedditPost posts))
        //    {
        //        mostUpVoted = posts.data.children?.OrderByDescending(p => p.data.ups).Take(10).ToList();
        //    }
        //    // Implement your logic to get posts with the most upvotes
        //    // You can use LINQ to order the posts by upvotes and take the top N
        //    return mostUpVoted;
        //}

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
            Dictionary<string, int> userPostDistribution;
            var _cache = _cacheUpdateService._cache;
            var userPostCount = new Dictionary<string, int>();

            if (_cache.TryGetValue("posts", out RedditPost posts))
            {
                // Implement your logic to get users with the most posts
                // You can use a dictionary to track the post count for each user

                foreach (var post in posts.data.children)
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
            }

            // Order the users by post count
            return userPostCount.OrderByDescending(u => u.Value).Take(10).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
