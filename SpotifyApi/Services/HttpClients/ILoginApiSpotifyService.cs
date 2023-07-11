using SpotifyApi.Models.Spotify;

namespace SpotifyApi.Services.HttpClients
{
    public interface ILoginApiSpotifyService
    {
        Task<TokenSpotify> Authenticate();
    }
}
