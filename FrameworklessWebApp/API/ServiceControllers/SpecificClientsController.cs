using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FrameworklessWebApp.Application.Exceptions;
using FrameworklessWebApp.Application.Models;
using FrameworklessWebApp.Application.Services;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class SpecificClientsController
    {
        private readonly ClientService _clientService;

        public SpecificClientsController(ClientService clientService)
        {
            _clientService = clientService;
        }
        
        
        public Response GetResponse(HttpListenerRequest request, string[] parameters)
        {
            try
            {
                var username = parameters[1];

                switch (request.HttpMethod)
                {
                    case "GET":
                        var client = _clientService.GetClientByUsername(username);
                        
                        return new Response(200, JsonConvert.SerializeObject(client));
                    
                    case "PUT":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var updatedClient = JsonConvert.DeserializeObject<Client>(body);
                        _clientService.UpdateClient(username, updatedClient);
                        return new Response(200, "Updated");

                    case "DELETE":
                        _clientService.DeleteClient(username);
                        return new Response(200, "Deleted");
                }

                return BasicResponseBuilder.GetBadRequest();
            }
            catch (ClientNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return new Response(404, e.Message);
            }
            
        }
    }
}