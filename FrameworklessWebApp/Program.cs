using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrameworklessWebApp.API;
using FrameworklessWebApp.API.ServiceControllers;
using FrameworklessWebApp.Data;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;

namespace FrameworklessWebApp
{
    class Program
    {
        private const string Port = "8080";
        private static readonly string Uri = $"http://localhost:{Port}/";

        
        static async Task Main(string[] args)
        {
            var retriever = new StubRetriever(GetStubbedClients());

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
        

        private static List<Client> GetStubbedClients()
        {
            var client1 = new Client("Braden", "Alsford")
            {
                JournalEntries = new List<JournalEntry>
                {
                    new JournalEntry(new DateTime(2020, 12, 1), "Braden's first entry")
                }
            };

            return new List<Client> { client1 };
        }
    }
}