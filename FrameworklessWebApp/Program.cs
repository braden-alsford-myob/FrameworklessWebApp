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


        static void Main(string[] args)
        {
            var retriever = new StubRetriever(GetStubbedClients(), GetStubbedJournalEntries());
            // var retriever = new DynamoRetriever();


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

            Console.WriteLine($"\n\nServer listening on port: {Port}");
            server.Run();

            Console.ReadKey();
        }

        private static List<Client> GetStubbedClients()
        {
            return new List<Client>
            {
                new Client("John", "Smith")
            };
        }


        private static List<JournalEntry> GetStubbedJournalEntries()
        {
            return new List<JournalEntry>
            {
                new JournalEntry(1, 0, DateTime.Now, "This is an entry!")
            };
        }
    }
}