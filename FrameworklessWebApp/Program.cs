using System;
using System.Threading.Tasks;
using FrameworklessWebApp.API;
using FrameworklessWebApp.API.ServiceControllers;
using FrameworklessWebApp.Application.Services;
using FrameworklessWebApp.Data;

namespace FrameworklessWebApp
{
    class Program
    {
        private const string Port = "8080";
        private static readonly string Uri = $"http://localhost:{Port}/";

        
        static async Task Main(string[] args)
        {
            var retriever = new StubRetriever();

            var clientsService = new ClientService(retriever);
            var journalEntryService = new JournalEntryService(retriever);

            var generalClientsController = new GeneralClientsController(clientsService);
            var specificClientsController = new SpecificClientsController(clientsService);
            var generalJournalEntryController = new GeneralJournalEntryController(journalEntryService);
            var specificJournalEntryController = new SpecificJournalEntryController(journalEntryService);
            

            var router = new Router(
                generalJournalEntryController, 
                specificJournalEntryController, 
                generalClientsController,
                specificClientsController);

            var server = new Server(Uri, router);

            Console.WriteLine($"Server listening on port: {Port}"); 
            await server.Run();
        }
    }
}