using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using FrameworklessWebApp.Services.Exceptions;
using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class GeneralClientsController : IController
    {
        private readonly ClientService _clientService;

        public GeneralClientsController(ClientService clientService)
        {
            _clientService = clientService;
        }
        
        public Response GetResponse(HttpListenerRequest request, string[] parameters)
        {
            try
            {
                switch (request.HttpMethod)
                {
                    case "GET":
                        var clients = _clientService.GetClients();

                        var clientViewModels = clients.Select(ClientViewModel.ConvertToViewModel).ToList();

                        return new Response(200,
                            JsonConvert.SerializeObject(clientViewModels, new JsonApiSerializerSettings()));


                    case "POST":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var clientViewModel = JsonConvert.DeserializeObject<ClientViewModel>(body);

                        var id = _clientService.AddClient(Client.ConvertToClient(clientViewModel));

                        var idVm = new IdViewModel{Id = id};
                        
                        return new Response(201, JsonConvert.SerializeObject(idVm, new JsonApiSerializerSettings()));
                }

                return BasicResponseBuilder.GetBadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response(400, "todo");
            }
        }
    }
}