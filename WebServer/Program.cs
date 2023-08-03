using System.Net;

namespace WebServer
{
    internal class Program
    {
        static string Main(string[] args)
        {
            HttpListener httpListener= new();
            httpListener.Prefixes.Add("http://localhost:8598/");
            Console.WriteLine("Demarrage du listener http");
            httpListener.Start();
            HttpListenerContext context = httpListener.GetContext();
            string code = context.Request.QueryString["code"];
            return code;
        }
    }
}