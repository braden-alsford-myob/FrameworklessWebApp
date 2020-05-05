using System.Linq;
using System.Net;
using FrameworklessWebApp.API.ServiceControllers;

namespace FrameworklessWebApp.API
{
    public class Router
    {
        private readonly GeneralJournalEntryController _generalJournalEntryController;
        private readonly SpecificJournalEntryController _specificJournalEntryController;
        private readonly ClientsController _clientsController;

        private const string JournalEntriesEndPoint = "journalEntries";
        private const string ClientsEndPoint = "clients";


        public Router(
            GeneralJournalEntryController generalJournalEntryController, 
            SpecificJournalEntryController specificJournalEntryController,
            ClientsController clientsController)
        {
            _generalJournalEntryController = generalJournalEntryController;
            _specificJournalEntryController = specificJournalEntryController;
            _clientsController = clientsController;
        }

        
        public Response ProcessRequest(HttpListenerRequest request)
        {
            var parameters = request.Url.Segments.Skip(1).Select(p => p.Replace("/", "")).ToArray();

            return parameters[0] switch
            {
                JournalEntriesEndPoint => HandleJournalEntries(request, parameters),
                ClientsEndPoint => _clientsController.GetResponse(request),
                _ => new Response(400, "Bad Request 😬")
            };
        }
        

        private Response HandleJournalEntries(HttpListenerRequest request, string[] parameters)
        {
            if (parameters.Length == 1)
            {
                return _generalJournalEntryController.GetResponse(request);
            }
            
            return _specificJournalEntryController.GetResponse(request, parameters);
        }
    }
}