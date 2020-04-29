using System;
using System.IO;
using System.Net;
using System.Threading;

namespace FrameworklessWebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpListener();
            var port = 8080;
            server.Prefixes.Add($"http://localhost:{port}/");
            server.Start();
            Console.WriteLine($"Server listening on port: {port}");
            
            
            while (true)
            {
                var context = server.GetContext();  // Gets the request
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");

                if (context.Request.HttpMethod == "POST")
                {
                    Console.WriteLine("Its a post!");
                    var body = new StreamReader(context.Request.InputStream).ReadToEnd();
                    Console.WriteLine(body);
                }
                
                var buffer = System.Text.Encoding.UTF8.GetBytes("Hello Braden");
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);  // forces send of response
            }
            server.Stop();  // never reached...
        }
    }
}