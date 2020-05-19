using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FrameworklessWebApp.API;
using FrameworklessWebApp.API.ServiceControllers;
using FrameworklessWebApp.Data;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using JsonApiSerializer;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestProject1
{
    public class EndpointTests
    {
        private const string ServerUri = "http://localhost:8081/";

        private List<Client> _clients;
        private List<JournalEntry> _journalEntries;
        
        private static HttpClient _client;
        private Server _server;
        

        [SetUp]
        public void Setup()
        {
            var client1 = new Client("Braden", "Alsford");

            _clients = new List<Client> { client1 };
            
            
            var journalEntry = new JournalEntry(
                1,
                1,
                new DateTime(2020, 1, 1),
                "Today I am grateful for...");
            
            _journalEntries = new List<JournalEntry>{ journalEntry };
            
            
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
            
            Task.Run(() => _server.Run());
            
            _client = new HttpClient();
        }
        

        [TearDown]
        public void TearDown()
        {
            // Pretty sure I need to stop the server here... Not sure how it actually works.
            // Probably the thing that is causing the runner to break. 
        }

        
        [Test]
        public void Test()
        {
            Assert.True(true);
        }

        
        [Test]
        public void Server_Can_Get_All_Clients()
        {
            var endPoint = ServerUri + "clients/";
            var expectedBody = JsonConvert.SerializeObject(_clients, new JsonApiSerializerSettings());
            
            var response = _client.GetAsync(endPoint).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }
        
        
        // [Test]
        // public async Task Server_Can_Create_A_New_Client()
        // {
        //     var endPoint = ServerUri + "clients/";
        //     
        //     var response = await _client.GetAsync(endPoint);
        //     var responseBody = await response.Content.ReadAsStringAsync();
        //     
        //     Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //     Assert.AreEqual(expectedBody, responseBody);
        // }
        
        

        
        // [Test]
        // public async Task Server_Can_Create_New_Journal_Entry()
        // {
        //     var endPoint = ServerUri + "journalEntries/";
        //     var requestBody = "{\"TimeAdded\": \"2020-04-30T12:09:24.437329+10:00\"," +
        //                       "\"Content\": \"Today I am grateful for coffee!\"}";
        //     
        //     var response = await _client.PostAsync(endPoint, new StringContent(requestBody));
        //     
        //     var responseBody = await response.Content.ReadAsStringAsync();
        //     Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        //     Assert.AreEqual("\"Id\" : 3", responseBody);
        // }
        //
        //
        // [Test]
        // public async Task Server_Will_Return_BadRequest_For_An_Unsupported_Endpoint()
        // {
        //     var endPoint = ServerUri + "asdf/";
        //     
        //     var response = await _client.GetAsync(endPoint);
        //     
        //     Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        // }
    }
}