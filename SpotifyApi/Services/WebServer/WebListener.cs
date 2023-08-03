using System.Net;

namespace SpotifyApi;
public class WebListener : IWebListener, IDisposable
{
    private readonly HttpListener _httpListener;

    public WebListener()
    {
        _httpListener = new();
    }

    public void Dispose()
    {
        Console.WriteLine("Arret du listener web");
        _httpListener.Stop();
    }

    public async Task<string> GetCode()
    {
        HttpListenerContext context = _httpListener.GetContext();
        HttpListenerResponse response = context.Response;
        string responseString;
        bool succes = true;
        if (context.Request.QueryString["error"] is not null)
        {
            responseString = "<html><body>Il faut accepter pour faire fonctionner l'app</body></html>";
            succes = false;
        }
        else if (context.Request.QueryString["code"] is null)
        {
            responseString = "<html><body>Erreur innatendue lors de l'autorisation</body></html>";
        }
        else
        {
            responseString = "<HTML><BODY>Autorisation OK. Vous pouvez retourner dans l'application</BODY></HTML>";
        }
        // Construct a response.
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
        if(succes)
        {
            return context.Request.QueryString["code"];
        }
        else
        {
            return string.Empty;
        }
    }

    public void StartListening()
    {
        _httpListener.Prefixes.Add("http://localhost:8598/");
        Console.WriteLine("Demarrage du listener http");
        _httpListener.Start();
    }
}
