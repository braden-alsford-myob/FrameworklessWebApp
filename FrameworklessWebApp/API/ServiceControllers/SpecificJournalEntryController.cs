using System;
using System.Net;
using FrameworklessWebApp.Application;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public class SpecificJournalEntryController
    {
        private readonly JournalEntryService _journalEntryService;
        
        
        public SpecificJournalEntryController(JournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }
        
        
        public Response GetResponse(HttpListenerRequest request, string[] parameters)
        {
            int entryId;

            if (!int.TryParse(parameters[1], out entryId))
            {
                return BasicResponseBuilder.GetBadRequest();
            }
            
            try
            {
                switch (request.HttpMethod)
                {
                    case "GET":
                        var journalEntry = _journalEntryService.GetEntryById(entryId);
                        return new Response(200, JsonConvert.SerializeObject(journalEntry));

                    case "DELETE":
                        _journalEntryService.DeleteEntry(entryId);
                        return new Response(200, "Deleted");
                }
            }
            
            catch (JournalEntryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return BasicResponseBuilder.GetNotFound();
            }

            return BasicResponseBuilder.GetBadRequest();
        }
    }
}