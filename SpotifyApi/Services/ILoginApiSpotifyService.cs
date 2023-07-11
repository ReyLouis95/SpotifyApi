using SpotifyApi.Models.Spotify;

namespace SpotifyApi.Services
{
    public interface ILoginApiSpotifyService
    {
        Task<TokenSpotify> Authenticate();
    }
}
