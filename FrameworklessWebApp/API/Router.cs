using System.Collections.Generic;
using System.Linq;
using System.Net;
using FrameworklessWebApp.API.ServiceControllers;

namespace FrameworklessWebApp.API
{
    public class Router
    {
        private readonly GeneralJournalEntryController _generalJournalEntryController;
        private readonly SpecificJournalEntryController _specificJournalEntryController;
        private readonly GeneralClientsController _generalClientsController;
        private readonly SpecificClientsController _specificClientsController;

        private const string ClientsEndPoint = "clients";
        private const string JournalEntriesEndPoint = "journalEntries";



        public Router(
            GeneralJournalEntryController generalJournalEntryController, 
            SpecificJournalEntryController specificJournalEntryController, 
            GeneralClientsController generalClientsController, 
            SpecificClientsController specificClientsController)
        {
            _generalJournalEntryController = generalJournalEntryController;
            _specificJournalEntryController = specificJournalEntryController;
            _generalClientsController = generalClientsController;
            _specificClientsController = specificClientsController;
        }

        
        public Response ProcessRequest(HttpListenerRequest request)
        {
            var parameters = request.Url.Segments.Skip(1).Select(p => p.Replace("/", "")).ToArray();
            
            if (parameters[0] == ClientsEndPoint)
            {
                if (parameters.Length > 2 && parameters[2] == JournalEntriesEndPoint)
                {
                    return HandleJournalEntries(request, parameters);
                }
                
                if (parameters.Length > 1)
                {
                    return _specificClientsController.GetResponse(request, parameters);
                }
                
                return _generalClientsController.GetResponse(request);
            }

            return new Response(400, "Bad Request");
        }
        

        private Response HandleJournalEntries(HttpListenerRequest request, string[] parameters)
        {
            if (parameters.Length == 3)
            {
                return _generalJournalEntryController.GetResponse(request, parameters);
            }
            
            return _specificJournalEntryController.GetResponse(request, parameters);
        }
        
        
        private Dictionary<string, string> GetQueryParameters(HttpListenerRequest request)
        {
            var queryParameters = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(request.Url.Query))
            {
                return queryParameters;
            }
            
            var queryParamsPairs = request.Url.Query.Substring(1).Split('&');
            
            foreach (var param in queryParamsPairs)
            {
                var splitParamPairs = param.Split('=');
                queryParameters.Add(splitParamPairs[0], splitParamPairs[1]);
            }

            return queryParameters;
        }
    }
}