namespace SpotifyApi;
public interface IWebListener
{
    void StartListening();
    Task<string> GetCode();
}
