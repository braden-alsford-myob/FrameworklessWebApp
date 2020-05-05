using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        
        
        public async Task Run()
        {
            _server.Start();
            
            while (true)
            {
                var context = _server.GetContext();
                await ProcessContext(context);
            }
        }

        private async Task ProcessContext(HttpListenerContext context)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");
                
                var response = _router.ProcessRequest(context.Request);

                var responseBuffer = System.Text.Encoding.UTF8.GetBytes(response.Body);
                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = responseBuffer.Length;
                context.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                context.Response.Headers.Add("custom", "hello");
            });
        }
    }
}