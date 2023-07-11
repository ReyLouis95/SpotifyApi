using SpotifyApi.Models.Config;
using SpotifyApi.Services.HttpClients;

namespace SpotifyApi.Gui
{
    public partial class MainPage : ContentPage
    {
        private readonly SpotifyCredentials _credentials;
        private readonly IApiSpotifyService _apiSpotifyService;

        public MainPage(SpotifyCredentials credentials, IApiSpotifyService apiSpotifyService)
        {
            _credentials = credentials;
            _apiSpotifyService = apiSpotifyService;
            InitializeComponent();

        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            string search = Search.Text;
            string reponse = await _apiSpotifyService.Search(search);

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}