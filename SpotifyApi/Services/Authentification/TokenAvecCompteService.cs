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
    private readonly Base64Service _base64Service;

    public TokenAvecCompteService(SpotifyCredentials credentials, IHttpClientFactory httpClientFactory, Base64Service base64Service)
    {
        _credentials = credentials;
        _httpClientLoginSpotify = httpClientFactory.CreateClient("loginApiSpotify");
        _base64Service = base64Service;
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
        string clientIdClientSecretBase64 = Base64Service.Base64Encode($"{_credentials.ClientId}:{_credentials.ClientSecret}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", clientIdClientSecretBase64);
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
            };
        Process.Start(new ProcessStartInfo(QueryHelpers.AddQueryString(url, queryParams)) { UseShellExecute = true });
    }
}
