using SpotifyApi.Models.Spotify;
using SpotifyApi.Services.HttpClients;
using System.Net.Http.Headers;

namespace SpotifyApi.Services.Handlers
{
    public class LoginHandler : DelegatingHandler
    {
        private readonly ILoginApiSpotifyService _loginApiSpotifyService;

        public LoginHandler(ILoginApiSpotifyService loginApiSpotifyService)
        {
            _loginApiSpotifyService = loginApiSpotifyService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TokenSpotify token = await _loginApiSpotifyService.Authenticate();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
