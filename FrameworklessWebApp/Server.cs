using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace FrameworklessWebApp
{
    public class Server
    {
        private readonly HttpListener _server;
        private readonly ClientService _clientService;

        public Server(string serverUri, ClientService clientService)
        {
            _clientService = clientService;
            
            _server = new HttpListener();
            _server.Prefixes.Add(serverUri);
        }
        
        
        public void Run()
        {
            _server.Start();
            
            while (true)
            {
                var context = _server.GetContext();
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");
                ProcessContext(context);
            }
        }
        
        
        private void ProcessContext(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                {
                    var responseBody = _clientService.GetClients();
                
                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseBody);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    break;
                }
                case "POST":
                {
                    var body = new StreamReader(context.Request.InputStream).ReadToEnd();
                    var newClient = JsonConvert.DeserializeObject<Client>(body);
            
                    _clientService.AddClient(newClient);
                
                    var buffer = System.Text.Encoding.UTF8.GetBytes("OK");
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.StatusCode = 404;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    break;
                }
            }
        }
    }
}