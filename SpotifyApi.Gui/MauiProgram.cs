﻿using Microsoft.Extensions.Configuration;
using SpotifyApi.Models.Config;
using SpotifyApi.Services.Authentification;
using SpotifyApi.Services.Handlers;
using SpotifyApi.Services.HttpClients;
using SpotifyApi.Services.Outils;
using System.Reflection;

namespace SpotifyApi.Gui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //config appsettings.json
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpotifyApi.Gui.appsettings.json");
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
            builder.Configuration.AddConfiguration(config);

            var credentials = builder.Configuration.GetSection(nameof(SpotifyCredentials)).Get<SpotifyCredentials>();
            var spotifyConfiguration = builder.Configuration.GetSection(nameof(SpotifyConfiguration)).Get<SpotifyConfiguration>();

            config.Bind(credentials);
            config.Bind(spotifyConfiguration);

            builder.Services.AddSingleton(credentials);
            builder.Services.AddSingleton(spotifyConfiguration);

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<ITokenCompteAnonymeService, TokenCompteAnonymeService>();
            builder.Services.AddSingleton<ITokenAvecCompteService, TokenAvecCompteService>();
            builder.Services.AddSingleton<IWebListener, WebListener>();
            builder.Services.AddSingleton<ILoginApiSpotifyService, LoginApiSpotifyService>();
            builder.Services.AddSingleton<Base64Service>();

            builder.Services.AddScoped<IApiSpotifyService, ApiSpotifyService>();

            builder.Services.AddHttpClient("loginApiSpotify", config =>
            {
                config.BaseAddress = new Uri(spotifyConfiguration.UrlApiLoginSpotify);
            }).AddHttpMessageHandler<AuthorizationHandler>();
            builder.Services.AddHttpClient("spotify", config =>
            {
                config.BaseAddress = new Uri(spotifyConfiguration.UrlApiSpotify);
            }).AddHttpMessageHandler<LoginHandler>();

            builder.Services.AddTransient<LoginHandler>();
            builder.Services.AddTransient<AuthorizationHandler>();

            return builder.Build();

        }
    }
}