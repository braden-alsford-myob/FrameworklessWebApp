using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using FrameworklessWebApp.API.ViewModels;
using FrameworklessWebApp.Application.Exceptions;
using FrameworklessWebApp.Application.Models;
using FrameworklessWebApp.Application.Services;
using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class GeneralClientsController
    {
        private readonly ClientService _clientService;

        public GeneralClientsController(ClientService clientService)
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
                        var clients = _clientService.GetClients();

                        var clientViewModels = clients.Select(ClientViewModel.ConvertToViewModel).ToList();

                        return new Response(200, JsonConvert.SerializeObject(clientViewModels, new JsonApiSerializerSettings()));


                    case "POST":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var clientViewModel = JsonConvert.DeserializeObject<ClientViewModel>(body);

                        _clientService.AddClient(Client.ConvertToClient(clientViewModel));

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