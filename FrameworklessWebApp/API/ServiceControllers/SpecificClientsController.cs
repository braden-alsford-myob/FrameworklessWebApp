using System;
using System.IO;
using System.Net;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using FrameworklessWebApp.Services.Exceptions;
using JsonApiSerializer;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class SpecificClientsController : IController
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
                var clientId = int.Parse(parameters[1]);

                switch (request.HttpMethod)
                {
                    case "GET":
                        var client = _clientService.GetClientById(clientId);
                        var clientVm = ClientViewModel.ConvertToViewModel(client);
                        
                        var responseBody = JsonConvert.SerializeObject(clientVm, new JsonApiSerializerSettings());

                        return new Response(200, responseBody);
                    
                    case "PUT":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var updatedClientVm = JsonConvert.DeserializeObject<ClientViewModel>(body);
                        var updatedClient = Client.ConvertToClient(updatedClientVm);

                        _clientService.UpdateClient(clientId, updatedClient);
                        
                        return new Response(200, "Updated");

                    case "DELETE":
                        _clientService.DeleteClient(clientId);
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