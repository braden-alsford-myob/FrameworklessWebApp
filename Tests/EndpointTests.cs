using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FrameworklessWebApp.API;
using FrameworklessWebApp.API.ServiceControllers;
using FrameworklessWebApp.Application;
using FrameworklessWebApp.Application.Models;
using FrameworklessWebApp.Application.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestProject1
{
    public class EndpointTests
    {
        private const string ServerUri = "http://localhost:8081/";
        
        private static HttpClient _client;

        private List<JournalEntry> _initialJournalEntries;

        private Server _server;
        private Thread _serverThread;
        
        
        [SetUp]
        public void Setup()
        {
            // _initialJournalEntries = new List<JournalEntry>
            // {
            //     new JournalEntry(1, new DateTime(2020, 4, 28), "Dear Journal..."),
            //     new JournalEntry(2, new DateTime(2020, 4, 29), "Dear Journal... x2")
            // };
            //
            // var journalEntryService = new JournalEntryService(_initialJournalEntries);
            //
            // var generalJournalEntryController = new GeneralJournalEntryController(journalEntryService);
            // var specificJournalEntryController = new SpecificJournalEntryController(journalEntryService);
            // var router = new Router(generalJournalEntryController, specificJournalEntryController);
            //
            // _server = new Server(ServerUri, router);
            //
            // _serverThread = new Thread(_server.Run);
            // _serverThread.Start();
            //
            // _client = new HttpClient();
        }
        

        [TearDown]
        public void TearDown()
        {
            // Pretty sure I need to stop the server here... Not sure how it actually works.
            // Probably the thing that is causing the runner to break. 
        }

        
        [Test]
        public async Task Server_Can_Get_All_Journal_Entries()
        {
            var endPoint = ServerUri + "journalEntries/";
            var expectedBody = JsonConvert.SerializeObject(_initialJournalEntries);
            
            var response = await _client.GetAsync(endPoint);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }

        
        [Test]
        public async Task Server_Can_Create_New_Journal_Entry()
        {
            var endPoint = ServerUri + "journalEntries/";
            var requestBody = "{\"TimeAdded\": \"2020-04-30T12:09:24.437329+10:00\"," +
                              "\"Content\": \"Today I am grateful for coffee!\"}";
            
            var response = await _client.PostAsync(endPoint, new StringContent(requestBody));
            
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual("\"Id\" : 3", responseBody);
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