namespace SpotifyApi.Services.HttpClients
{
    public class ApiSpotifyService : IApiSpotifyService
    {
        private readonly HttpClient _httpClientSpotify;

        public ApiSpotifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientSpotify = httpClientFactory.CreateClient("spotify");
        }

        public async Task<string> Search(string search)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, $"search?q=artist:{search}&type=artist");
            HttpResponseMessage response = await _httpClientSpotify.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
