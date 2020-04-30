using System;
using System.IO;
using System.Linq;
using System.Net;
using FrameworklessWebApp.Application;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API
{
    public class Server
    {
        private readonly HttpListener _server;
        private readonly Router _router;

        public Server(string serverUri, Router router)
        {
            _router = router;
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
            var response = _router.ProcessRequest(context.Request);
            
            var responseBuffer = System.Text.Encoding.UTF8.GetBytes(response.Body);
            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = responseBuffer.Length;
            context.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
        }
    }
}