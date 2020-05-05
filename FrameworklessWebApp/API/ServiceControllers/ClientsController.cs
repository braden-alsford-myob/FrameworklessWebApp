using System;
using System.IO;
using System.Net;
using FrameworklessWebApp.Application;
using FrameworklessWebApp.Application.Exceptions;
using FrameworklessWebApp.Application.Models;
using FrameworklessWebApp.Application.Services;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class ClientsController
    {
        private readonly ClientService _clientService;

        public ClientsController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public Response GetResponse(HttpListenerRequest request)
        {
            try
            {
                switch (request.HttpMethod)
                {
                    case "GET":
                        var journalEntries = _clientService.GetClients();

                        return new Response(200, JsonConvert.SerializeObject(journalEntries));
                    case "POST":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var newClient = JsonConvert.DeserializeObject<Client>(body);

                        _clientService.AddClient(newClient);
                        
                        return new Response(201, "Created");
                }

                return BasicResponseBuilder.GetBadRequest();
            }
            catch (NameTakenException e)
            {
                Console.WriteLine(e.Message);
                return new Response(400, e.Message);
            }
            
        }
    }
}