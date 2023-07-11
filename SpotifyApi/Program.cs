using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyApi.Models.Config;
using SpotifyApi.Services.Handlers;
using SpotifyApi.Services.HttpClients;
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

            builder.Services.AddTransient<LoginHandler>();

            //http clients
            builder.Services.AddHttpClient("loginApiSpotify", config =>
            {
                config.BaseAddress = new Uri(spotifyConfiguration.UrlApiLoginSpotify);
            });
            builder.Services.AddHttpClient("spotify", config =>
            {
                config.BaseAddress = new Uri(spotifyConfiguration.UrlApiSpotify);
            }).AddHttpMessageHandler<LoginHandler>();

            builder.Services.AddSingleton<ILoginApiSpotifyService, LoginApiSpotifyService>();
            builder.Services.AddSingleton<ApiSpotifyService>();
            using IHost host = builder.Build();

        }

    }
}