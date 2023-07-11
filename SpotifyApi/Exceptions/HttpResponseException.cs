using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApi.Exceptions
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public HttpResponseException()
        {
        }

        public HttpResponseException(HttpResponseMessage response, string responseString)
            :base($"Erreur Appel Http: Status Code: {response.StatusCode} - ReasonPhrase: {response.ReasonPhrase} - ResponseString: {responseString} - RequestMessage: {response.RequestMessage}")
        {
        }
    }
}
