using System;
using code_challenge_reddit.Model;
using code_challenge_reddit.Services;

namespace code_challenge_reddit
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IRedditStatisticService, RedditStatisticService>();
            services.AddSingleton<IRedditFeedService, RedditFeedService>();
            services.Configure<CacheSettings>(Configuration.GetSection("CacheSettings"));
            services.Configure<RedditFeedSettings>(Configuration.GetSection("RedditFeedSettings"));
            services.AddSingleton<CacheUpdateService>();
            services.AddHostedService<CacheUpdateService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}
