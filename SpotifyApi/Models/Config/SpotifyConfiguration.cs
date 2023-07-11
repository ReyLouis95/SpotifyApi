using System.ComponentModel.DataAnnotations;

namespace SpotifyApi.Models.Config
{
    public class SpotifyConfiguration
    {
        [Required]
        public string UrlApiLoginSpotify { get;set; }
        [Required]
        public string UrlApiSpotify { get; set; }
    }
}
