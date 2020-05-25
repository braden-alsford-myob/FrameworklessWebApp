using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FrameworklessWebApp.API;
using FrameworklessWebApp.API.ServiceControllers;
using FrameworklessWebApp.Data;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using NUnit.Framework;

namespace TestProject1.Component
{
    public class UnhappyServerTests
    {
        private const string ServerUri = "http://localhost:8081/";

        private List<Client> _clients;
        private List<JournalEntry> _journalEntries;
        
        private static HttpClient _client;
        private Server _server;
        

        [SetUp]
        public void Setup()
        {
            var client1 = new Client(1, "John", "Smith");
            var client2 = new Client(2, "Bobby", "Brown");
            

            _clients = new List<Client> { client1, client2 };
            
            
            var journalEntry1 = new JournalEntry(
                1,
                1,
                new DateTime(2020, 1, 1),
                "Today I am grateful for...");      
            
            var journalEntry2 = new JournalEntry(
                2,
                1,
                new DateTime(2020, 1, 2),
                "Today I am grateful for something else.");
            
            _journalEntries = new List<JournalEntry>{ journalEntry1, journalEntry2 };
            
            
            var retriever = new StubRetriever(_clients, _journalEntries);
            
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
            
            _server = new Server(ServerUri, router);

            _server.RunAsync();
            
            _client = new HttpClient();
        }
        
        
        [Test]
        public async Task Server_Will_Return_BadRequest_For_An_Unsupported_Endpoint()
        {
            var endPoint = ServerUri + "asdf/";
            
            var response = await _client.GetAsync(endPoint);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        


    }
}