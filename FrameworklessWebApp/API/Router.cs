using System.Linq;
using System.Net;
using FrameworklessWebApp.Application;
using Newtonsoft.Json;

namespace FrameworklessWebApp.API
{
    public class Router
    {
        private readonly JournalEntryService _journalEntryService;
        
        private const string NotesEndPoint = "journalEntries";


        public Router(JournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }


        public Response ProcessRequest(HttpListenerRequest request)
        {
            var parameters = request.Url.Segments.Skip(1).Select(p => p.Replace("/", "")).ToArray();

            return parameters[0] switch
            {
                NotesEndPoint => HandleJournalEntries(request),
                _ => new Response(400, "Bad Request 😬")
            };
        }
        

        private Response HandleJournalEntries(HttpListenerRequest request)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                {
                    var journalEntries = _journalEntryService.GetEntries();
                    return new Response(200, JsonConvert.SerializeObject(journalEntries));
                }
            }
            
            return new Response(400, "Bad Request 😬");
        }
    }
}