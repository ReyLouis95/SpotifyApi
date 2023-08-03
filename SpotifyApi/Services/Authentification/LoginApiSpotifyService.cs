using SpotifyApi.Models.Config;
using SpotifyApi.Models.Spotify;
using SpotifyApi.Services.HttpClients;

namespace SpotifyApi.Services.Authentification
{
    public class LoginApiSpotifyService : ILoginApiSpotifyService
    {
        private TokenSpotify _token;
        private DateTime _lastUpdateToken;
        private readonly ITokenAvecCompteService _tokenAvecCompteService;
        private readonly ITokenCompteAnonymeService _tokenCompteAnonymeService;
        private readonly IWebListener _webListener;
        private readonly SpotifyConfiguration _spotifyConfiguration;

        public LoginApiSpotifyService(ITokenAvecCompteService tokenAvecCompteService, ITokenCompteAnonymeService tokenCompteAnonymeService, IWebListener webListener, SpotifyConfiguration spotifyConfiguration)
        {
            _tokenAvecCompteService = tokenAvecCompteService;
            _tokenCompteAnonymeService = tokenCompteAnonymeService;
            _webListener = webListener;
            _spotifyConfiguration = spotifyConfiguration;
        }

        private bool IsTokenValid()
        {
            double time = Convert.ToDouble(_token.ExpiresIn);
            return !(DateTime.Now.AddSeconds(-1 * time) > _lastUpdateToken);
        }

        public async Task<TokenSpotify> AuthenticateAnonyme()
        {
            if (_token == null || string.IsNullOrEmpty(_token.AccessToken) || _token.ExpiresIn == null)
            {
                await _tokenCompteAnonymeService.Authenticate();
            }
            if (!IsTokenValid())
            {
                await _tokenCompteAnonymeService.Authenticate();
            }
            return _token;
        }

        public async Task<TokenSpotify> AuthenticateAvecCompte()
        {
            if (_token == null || string.IsNullOrEmpty(_token.AccessToken) || _token.ExpiresIn == null)
            {
                _webListener.StartListening();
                _tokenAvecCompteService.OpenDialogAuthent(_spotifyConfiguration.UrlDialogAuthent);
                string code = await _webListener.GetCode();
                var resultat = await _tokenAvecCompteService.GetToken(code);
                _token = resultat.Item1;
                _lastUpdateToken = resultat.Item2;
            }
            else if(!IsTokenValid())
            {
                var resultat = await _tokenAvecCompteService.GetRefreshToken(_token.RefreshToken);
                _token = resultat.Item1;
                _lastUpdateToken = resultat.Item2;
            }
            return _token;
        }
    }
}
