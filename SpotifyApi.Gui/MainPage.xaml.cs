using SpotifyApi.Models.Config;
using SpotifyApi.Services.HttpClients;

namespace SpotifyApi.Gui
{
    public partial class MainPage : ContentPage
    {
        private readonly IApiSpotifyService _apiSpotifyService;

        public MainPage(IApiSpotifyService apiSpotifyService)
        {
            _apiSpotifyService = apiSpotifyService;
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            string search = Search.Text;
            var reponse = await _apiSpotifyService.Search(search);

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}