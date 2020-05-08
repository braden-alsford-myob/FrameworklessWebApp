using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using FrameworklessWebApp.Services.Exceptions;
using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
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
                        var clientVm = ClientViewModel.ConvertToViewModel(client);
                        
                        var responseBody = JsonConvert.SerializeObject(clientVm, new JsonApiSerializerSettings());

                        return new Response(200, responseBody);
                    
                    case "PUT":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var updatedClientVm = JsonConvert.DeserializeObject<ClientViewModel>(body);
                        var updatedClient = Client.ConvertToClient(updatedClientVm);

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