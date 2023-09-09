using SpotifyApi.Services.Authentification;
using SpotifyApi.Services.HttpClients;

namespace SpotifyApi.Gui
{
    public partial class MainPage : ContentPage
    {
        private readonly IApiSpotifyService _apiSpotifyService;
        private readonly ILoginApiSpotifyService _loginApiSpotifyService;

        public MainPage(IApiSpotifyService apiSpotifyService, ILoginApiSpotifyService loginApiSpotifyService)
        {
            _apiSpotifyService = apiSpotifyService;
            _loginApiSpotifyService = loginApiSpotifyService;
            InitializeComponent();
            _loginApiSpotifyService.AuthenticateAvecCompte();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            string search = Search.Text;
            var reponse = await _apiSpotifyService.Search(search);
            var reponse2 = await _apiSpotifyService.GetTopTracks();
            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}