using System;
using System.Collections.Generic;

namespace FrameworklessWebApp
{
    class Program
    {
        private const string Port = "8080";
        private static readonly string Uri = $"http://localhost:{Port}/";

        
        static void Main(string[] args)
        {
            var client1 = new Client(
                "Braden", 
                22, 
                new List<string>{"Skiing", "Surfing"});
            
            var clients = new List<Client>{client1};
            
            var clientService = new ClientService(clients);
            
            var server = new Server(Uri, clientService);
            server.Run();
            
            Console.WriteLine($"Server listening on port: {Port}");
        }
    }
}