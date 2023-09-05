using System.Text;
using code_challenge_reddit.Model;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace code_challenge_reddit.Services
{
    public interface IRedditFeedService
	{
		Task<RedditPost> GetRedditPosts(string subreddit);
	}

    public class RedditFeedService : IRedditFeedService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string RedditBaseUrl = "https://www.reddit.com/api/";
        private const string OauthBaseUrl = "https://oauth.reddit.com/r/";
        private string _clientId;
        private string _clientSecret;
        private TokenResponse _token;
        private string _userName;
        private string _password; 
        private string _applicationName;

        public RedditFeedService(IOptions<RedditFeedSettings> redditFeedSettings) 
        {
            _clientId = redditFeedSettings.Value.ClientId;
            _clientSecret = redditFeedSettings.Value.ClientSecret;
            _userName = redditFeedSettings.Value.UserName;
            _password = redditFeedSettings.Value.Password;
            _applicationName = redditFeedSettings.Value.ApplicationName;
        }

        public async Task<RedditPost> GetRedditPosts(string subreddit)
        {
            RedditPost result = null; 
            
            try
            {
                // Reddit API request and response handling here
                if (!IsValidToken(_token))
                await GetAccessTokenAsync();

                if (IsValidToken(_token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", $"{_applicationName}/0.1 by {_userName}");
                    //var response = await _httpClient.GetStringAsync($"{RedditBaseUrl}{subreddit}/about");
                    var response = await _httpClient.GetStringAsync($"{OauthBaseUrl}{subreddit}/new.json?limit=100");
                    //var response = await _httpClient.GetStringAsync("https://oauth.reddit.com/api/v1/me");
                    result = ParsePosts(response);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error)
                Console.WriteLine($"Error: {ex.Message}");
            }

            return result;
        }

        public bool IsValidToken(TokenResponse tokenResponse)
        {
            if (tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.access_token))
            {
                return false; // Access token is null or empty
            }

            // Check if the token has expired
            if (IsTokenExpired(tokenResponse))
            {
                _token = null;
                return false; // Access token has expired
            }

            return true; // Access token is valid
        }

        private bool IsTokenExpired(TokenResponse tokenResponse)
        {
            if (tokenResponse != null && tokenResponse.expires_in > 0)
            {
                var expirationTime = DateTime.Now.AddSeconds(tokenResponse.expires_in);
                if (expirationTime <= DateTime.Now)
                {
                    return true; // Token is expired
                }
            }

            return false;
        }

        private RedditPost ParsePosts(string response)
        {
            var post = JsonConvert.DeserializeObject<RedditPost>(response);
            return post;
        }
        private async Task GetAccessTokenAsync()
        {
            var _httpClient = new HttpClient();
            var clientAuth = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}"))
            );

            var post_data = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", _userName },
                { "password", _password }
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"{_applicationName}/0.1 by {_userName}");
            _httpClient.DefaultRequestHeaders.Authorization = clientAuth;

            var content = new FormUrlEncodedContent(post_data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await _httpClient.PostAsync(
                $"{RedditBaseUrl}v1/access_token",
                content
            );

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // Deserialize the JSON response to extract the access token
                var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseContent);

                if (tokenResponse != null)
                {
                    _token = tokenResponse;
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
                }
            }
        }

        static void TrackStatistics(List<Child> posts)
        {
            // Implement your logic to track statistics here
            // You can iterate through the posts and update your statistics accordingly
        }
    }
}

