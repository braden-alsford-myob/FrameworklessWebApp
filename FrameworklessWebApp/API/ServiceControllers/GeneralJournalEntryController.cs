using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class GeneralJournalEntryController : IController
    {
        private readonly JournalEntryService _journalEntryService;

        
        public GeneralJournalEntryController(JournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }
        
        
        public Response GetResponse(HttpListenerRequest request, string[] parameters)
        {
            var clientId = int.Parse(parameters[1]);

            switch (request.HttpMethod)
            {
                case "GET":
                    var journalEntries = _journalEntryService.GetEntries(clientId);

                    var journalEntryViewModels =
                        journalEntries.Select(JournalEntryViewModel.ConvertToViewModel).ToList();

                    return new Response(200, JsonConvert.SerializeObject(journalEntryViewModels, new JsonApiSerializerSettings()));
                
                case "POST":
                    var body = new StreamReader(request.InputStream).ReadToEnd();
                    var newJournalEntryVm = JsonConvert.DeserializeObject<JournalEntryViewModel>(body);
                    var newJournalEntry = JournalEntry.ConvertToJournalEntry(newJournalEntryVm);
                    
                    var newId = _journalEntryService.AddEntry(clientId, newJournalEntry);
                    var responseBody = JsonConvert.SerializeObject(new IdViewModel{Id = newId}, new JsonApiSerializerSettings());
                    
                    return new Response(201, responseBody);
            }

            return BasicResponseBuilder.GetBadRequest();
        }
    }
}