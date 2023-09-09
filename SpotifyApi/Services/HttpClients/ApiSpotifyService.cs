using SpotifyApi.Models.Spotify;
using System.Runtime.InteropServices;
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

        public async Task<IEnumerable<Artist>?> Search(string search)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, $"search?q=artist:{search}&type=artist");
            HttpResponseMessage response = await _httpClientSpotify.SendAsync(request);
            var listeArtistes = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("artists").GetProperty("items");
            return JsonSerializer.Deserialize<IEnumerable<Artist>>(listeArtistes);
        }

        public async Task<IEnumerable<Track>?> GetTopTracks()
        {
            using HttpRequestMessage request = new(HttpMethod.Get, $"me/top/tracks?limit=10");
            HttpResponseMessage response = await _httpClientSpotify.SendAsync(request);
            var stsring = await response.Content.ReadAsStringAsync();
            var listeTracks = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("items");
            return JsonSerializer.Deserialize<IEnumerable<Track>>(listeTracks);
        }
    }
}
