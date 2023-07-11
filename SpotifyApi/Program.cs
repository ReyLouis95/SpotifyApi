using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyApi.Models.Config;
using SpotifyApi.Services;
using System.Reflection;

namespace SpotifyApi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            //Ajout secret.json
            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
            //ajout credentials avec option pattern
            builder.Services.AddOptions<SpotifyCredentials>().BindConfiguration(nameof(SpotifyCredentials));
            builder.Services.AddOptions<SpotifyConfiguration>().BindConfiguration(nameof(SpotifyConfiguration));
            SpotifyConfiguration spotifyConfiguration = builder.Configuration.GetSection(nameof(SpotifyConfiguration)).Get<SpotifyConfiguration>();
            builder.Services.AddHttpClient("loginApiSpotify", config =>
            {
                config.BaseAddress = new Uri(spotifyConfiguration.UrlApiLoginSpotify);
            });
            builder.Services.AddSingleton<LoginApiSpotifyService>();
            using IHost host = builder.Build();
            LoginApiSpotifyService loginApiSpotifyService = host.Services.GetRequiredService<LoginApiSpotifyService>();
            

        }

    }
}