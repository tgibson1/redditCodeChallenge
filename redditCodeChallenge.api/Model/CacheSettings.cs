using System;
namespace code_challenge_reddit.Model
{
    public class CacheSettings
    {
        public TimeSpan CacheDuration { get; set; }
        public TimeSpan CacheUpdateInterval { get; set; }
    }
}