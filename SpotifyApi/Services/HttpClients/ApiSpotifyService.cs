﻿using SpotifyApi.Models.Spotify;
using System.Text.Json;

namespace SpotifyApi.Services.HttpClients
{
    public class ApiSpotifyService : IApiSpotifyService
    {
        private readonly HttpClient _httpClientSpotify;

        public ApiSpotifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientSpotify = httpClientFactory.CreateClient("spotify");
        }

        public async Task<IEnumerable<Artist>> Search(string search)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, $"search?q=artist:{search}&type=artist");
            HttpResponseMessage response = await _httpClientSpotify.SendAsync(request);
            var listeArtistes = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("artists").GetProperty("items");
            return JsonSerializer.Deserialize<IEnumerable<Artist>>(listeArtistes);
        }
    }
}
