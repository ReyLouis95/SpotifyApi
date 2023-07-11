using SpotifyApi.Models.Spotify;

namespace SpotifyApi.Services.HttpClients
{
    public interface IApiSpotifyService
    {
        Task<IEnumerable<Artist>> Search(string search);
    }
}
