using SpotifyApi.Services.Outils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SpotifyApi.Models.Config;

namespace SpotifyApi.Services.Handlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly SpotifyCredentials _credentials;

        public AuthorizationHandler(SpotifyCredentials credentials)
        {
            _credentials = credentials;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string clientIdClientSecretBase64 = Base64Service.Base64Encode($"{_credentials.ClientId}:{_credentials.ClientSecret}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", clientIdClientSecretBase64);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
