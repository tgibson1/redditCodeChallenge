using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using code_challenge_reddit.Model;

namespace code_challenge_reddit.Services
{
    public class CacheUpdateService : BackgroundService
    {
        public readonly IMemoryCache _cache;

        private readonly IRedditFeedService _redditFeedService;
        private readonly ILogger<CacheUpdateService> _logger;
        private readonly CacheSettings _cacheSettings;

        private Timer _cacheUpdateTimer;

        public CacheUpdateService(
            IRedditFeedService redditFeedService,
            ILogger<CacheUpdateService> logger,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings)
        {
            _redditFeedService = redditFeedService;
            _logger = logger;
            _cache = cache;
            _cacheSettings = cacheSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cacheUpdateTimer = new Timer(
                async state => await UpdateCache(),
                null,
                TimeSpan.Zero,
                _cacheSettings.CacheUpdateInterval);

            await Task.Delay(-1, stoppingToken); // Keep the service running
        }

        private async Task UpdateCache()
        {
            try
            {
                var redirects = _redditFeedService.GetRedditPosts();
                _cache.Set(
                    "redirects",
                    redirects,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _cacheSettings.CacheDuration
                    });
                _logger.LogInformation("Cache updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update cache.");
            }
        }
    }
}