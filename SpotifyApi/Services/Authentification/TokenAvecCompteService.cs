using System.Diagnostics;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using SpotifyApi.Exceptions;
using SpotifyApi.Models.Config;
using SpotifyApi.Models.Spotify;
using System.Net.Http.Headers;
using SpotifyApi.Services.Outils;

namespace SpotifyApi;
public class TokenAvecCompteService : ITokenAvecCompteService
{
    private readonly SpotifyCredentials _credentials;
    private readonly HttpClient _httpClientLoginSpotify;

    public TokenAvecCompteService(SpotifyCredentials credentials, IHttpClientFactory httpClientFactory)
    {
        _credentials = credentials;
        _httpClientLoginSpotify = httpClientFactory.CreateClient("loginApiSpotify");
    }

    public async Task<(TokenSpotify?, DateTime)> GetRefreshToken(string refreshToken)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "refresh_token"},
            {"refresh_token", refreshToken},
        });
        HttpResponseMessage response = await _httpClientLoginSpotify.SendAsync(request);
        if(response.StatusCode != HttpStatusCode.OK)
        {
            throw new HttpResponseException(response, await response.Content.ReadAsStringAsync());
        }
        TokenSpotify? tokenSpotify = await response.Content.ReadFromJsonAsync<TokenSpotify>();
        if (tokenSpotify == null)
        {
            throw new ArgumentNullException(nameof(tokenSpotify));
        }
        tokenSpotify.RefreshToken = refreshToken;
        return (tokenSpotify, DateTime.Now);
    }

    public async Task<(TokenSpotify?, DateTime)> GetToken(string code)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code",  code},
                { "redirect_uri", _credentials.RedirectUrl },
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


    public void OpenDialogAuthent(string url)
    {
        var queryParams = new Dictionary<string, string>()
            {
                {"client_id", _credentials.ClientId},
                {"response_type", "code" },
                {"redirect_uri", _credentials.RedirectUrl },
                {"scope", "user-read-private user-read-email user-top-read" }
            };
        Process.Start(new ProcessStartInfo(QueryHelpers.AddQueryString(url, queryParams)) { UseShellExecute = true });
    }
}
