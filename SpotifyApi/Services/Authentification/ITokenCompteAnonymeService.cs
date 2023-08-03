using SpotifyApi.Models.Spotify;

namespace SpotifyApi.Services.HttpClients
{
    /// <summary>
    /// Permet d'obtenir un token d'authent sans que l'utilisateur ne renseigne son compte
    /// </summary>
    public interface ITokenCompteAnonymeService
    {
        Task<(TokenSpotify, DateTime)> Authenticate();
    }
}
