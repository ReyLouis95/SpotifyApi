using Microsoft.Extensions.Options;
using SpotifyApi.Exceptions;
using SpotifyApi.Models.Config;
using SpotifyApi.Models.Spotify;
using System.Net;
using System.Net.Http.Json;

namespace SpotifyApi.Services.HttpClients
{
    public class LoginApiSpotifyService : ILoginApiSpotifyService
    {
        private readonly HttpClient _httpClientLoginSpotify;
        private readonly SpotifyCredentials _credentials;
        private TokenSpotify _token;
        private DateTime _lastUpdateToken;

        public LoginApiSpotifyService(IHttpClientFactory httpClientFactory, IOptions<SpotifyCredentials> credentials)
        {
            _httpClientLoginSpotify = httpClientFactory.CreateClient("loginApiSpotify");
            _credentials = credentials.Value;
        }

        public async Task<TokenSpotify> Authenticate()
        {
            if (_token == null || string.IsNullOrEmpty(_token.AccessToken) || _token.ExpiresIn == null)
            {
                await GetToken();
            }
            if (!IsTokenValid())
            {
                await GetToken();
            }
            return _token;
        }

        private async Task GetToken()
        {
            using HttpRequestMessage request = new(HttpMethod.Post, "token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id",  _credentials.ClientId},
                { "client_secret", _credentials.ClientSecret },
            });
            HttpResponseMessage response = await _httpClientLoginSpotify.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response, await response.Content.ReadAsStringAsync());
            }
            TokenSpotify? tokenSpotify = await response.Content.ReadFromJsonAsync<TokenSpotify>();
            if (tokenSpotify == null)
            {
                throw new ArgumentNullException(nameof(tokenSpotify));
            }
            _lastUpdateToken = DateTime.Now;
            _token = tokenSpotify;
        }

        private bool IsTokenValid()
        {
            double time = Convert.ToDouble(_token.ExpiresIn);
            return !(DateTime.Now.AddSeconds(-1 * time) > _lastUpdateToken);
        }
    }
}
