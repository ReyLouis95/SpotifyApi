namespace SpotifyApi.Services.HttpClients
{
    public interface IApiSpotifyService
    {
        Task<string> Search(string search);
    }
}
