using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using SpotifyApi.Exceptions;
using SpotifyApi.Models.Config;
using SpotifyApi.Models.Spotify;

namespace SpotifyApi.Services.HttpClients
{
    public class TokenCompteAnonymeService : ITokenCompteAnonymeService
    {
        private readonly HttpClient _httpClientLoginSpotify;
        private readonly SpotifyCredentials _credentials;

        public TokenCompteAnonymeService(IHttpClientFactory httpClientFactory, SpotifyCredentials credentials)
        {
            _httpClientLoginSpotify = httpClientFactory.CreateClient("loginApiSpotify");
            _credentials = credentials;
        }

        public async Task<(TokenSpotify, DateTime)> Authenticate()
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
            return (tokenSpotify, DateTime.Now);
        }
    }
}
