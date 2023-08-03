using SpotifyApi.Models.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Services.Authentification
{
    public interface ILoginApiSpotifyService
    {
        Task<TokenSpotify> AuthenticateAnonyme();
        Task<TokenSpotify> AuthenticateAvecCompte();
    }
}
