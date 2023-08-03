using SpotifyApi.Models.Spotify;

namespace SpotifyApi;
/// <summary>
/// Permet de s'authentifier avec le compte de l'utilisateur
/// </summary>
public interface ITokenAvecCompteService
{
    void OpenDialogAuthent(string url);
    Task<(TokenSpotify?, DateTime)> GetToken(string code);
}
