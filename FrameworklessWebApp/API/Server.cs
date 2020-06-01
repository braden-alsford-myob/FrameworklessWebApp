using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworklessWebApp.API
{
    public class Server
    {
        private readonly HttpListener _server;
        private readonly Router _router;

        private bool _running;
        
        public Server(string serverUri, Router router)
        {
            _router = router;
            _server = new HttpListener();
            _server.Prefixes.Add(serverUri);

            _running = true;
        }


        public Task RunAsync()
        {
            return Task.Run(async () =>
            {
                _server.Start();
                while (_running)
                {
                    var context = await _server.GetContextAsync();
                    ProcessContext(context);
                }
            });
        }
        
        
        public async void Run()
        {
            _server.Start();
            
            while (_running)
            {
                var context = await _server.GetContextAsync();
                ProcessContext(context);
            }
        }

        private async void ProcessContext(HttpListenerContext context)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");

                var response = _router.ProcessRequest(context.Request);

                var responseBuffer = System.Text.Encoding.UTF8.GetBytes(response.Body);
                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/vnd.api+json";
                context.Response.ContentLength64 = responseBuffer.Length;
                context.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
                context.Response.OutputStream.Flush();
                context.Response.OutputStream.Close();
            });
        }

        public void Stop()
        {
            _running = false;
            _server.Stop();
        }
    }
}