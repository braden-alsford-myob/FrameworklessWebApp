using System.IO;
using System.Net;
using FrameworklessWebApp.Application.Models;
using FrameworklessWebApp.Application.Services;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class GeneralJournalEntryController
    {
        private readonly JournalEntryService _journalEntryService;

        
        public GeneralJournalEntryController(JournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }
        
        
        public Response GetResponse(HttpListenerRequest request, string[] parameters)
        {
            var clientUsername = parameters[1];

            switch (request.HttpMethod)
            {
                case "GET":
                    var journalEntries = _journalEntryService.GetEntries(clientUsername);
                    
                    return new Response(200, JsonConvert.SerializeObject(journalEntries));
                
                case "POST":
                    var body = new StreamReader(request.InputStream).ReadToEnd();
                    var newJournalEntry = JsonConvert.DeserializeObject<JournalEntry>(body);

                    var newId = _journalEntryService.AddEntry(clientUsername, newJournalEntry);
                    
                    return new Response(201, $"\"Id\" : {newId}");
            }

            return BasicResponseBuilder.GetBadRequest();
        }
    }
}