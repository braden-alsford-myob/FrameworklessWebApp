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
    public class SpecificJournalEntryController : IController
    {
        private readonly JournalEntryService _journalEntryService;
        
        
        public SpecificJournalEntryController(JournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }
        
        
        public Response GetResponse(HttpListenerRequest request, string[] parameters)
        {
            int entryId;

            if (!int.TryParse(parameters[3], out entryId))
            {
                return BasicResponseBuilder.GetBadRequest();
            }

            var clientId = int.Parse(parameters[1]);

            try
            {
                switch (request.HttpMethod)
                {
                    case "GET":
                        var journalEntry = _journalEntryService.GetEntryById(clientId, entryId);

                        var journalEntryVm = JournalEntryViewModel.ConvertToViewModel(journalEntry);
                        
                        var jsonBody =
                            JsonConvert.SerializeObject(journalEntryVm, new JsonApiSerializerSettings());
                        
                        return new Response(200, jsonBody);

                    case "PUT":
                        var body = new StreamReader(request.InputStream).ReadToEnd();
                        var updatedJournalVm = JsonConvert.DeserializeObject<JournalEntryViewModel>(body);
                        var updatedJournal = JournalEntry.ConvertToJournalEntry(updatedJournalVm);
                        _journalEntryService.UpdateEntry(clientId, entryId, updatedJournal);
                        return new Response(200, "Updated");

                    case "DELETE":
                        _journalEntryService.DeleteEntry(clientId, entryId);
                        return new Response(200, "Deleted");
                }
            }
            catch (JournalEntryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return BasicResponseBuilder.GetNotFound();
            }
            catch (MissingJournalEntryAttributesException e)
            {
                Console.WriteLine(e.Message);
                return BasicResponseBuilder.GetBadRequest();
            }

            return BasicResponseBuilder.GetBadRequest();
        }
    }
}