using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FrameworklessWebApp.API;
using FrameworklessWebApp.API.ServiceControllers;
using FrameworklessWebApp.Data;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services;
using JsonApiSerializer;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestProject1.Component
{
    public class HappyClientServerTests
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
        
            var router = new Router(
                new GeneralJournalEntryController(journalEntryService), 
                new SpecificJournalEntryController(journalEntryService), 
                new GeneralClientsController(clientsService), 
                new SpecificClientsController(clientsService));
            
            _server = new Server(ServerUri, router);
            
            _server.RunAsync();
            
            _client = new HttpClient();
        }
        
        
        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }


        [Test]
        public async Task Server_Can_Get_All_Clients()
        {
            var endPoint = ServerUri + "clients/";
            var clientsAsViewModels = _clients.Select(ClientViewModel.ConvertToViewModel).ToList();
            var expectedBody = JsonConvert.SerializeObject(clientsAsViewModels, new JsonApiSerializerSettings());
            
            var response = await _client.GetAsync(endPoint);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }
        

        [TestCase(0)]
        [TestCase(1)]
        public async Task Server_Can_Get_Specific_Client(int clientIndex)
        {
            var clientToGet = _clients[clientIndex];
            var endPoint = ServerUri + "clients/" + clientToGet.ClientID;
            var clientViewModel = ClientViewModel.ConvertToViewModel(clientToGet);
            var expectedBody = JsonConvert.SerializeObject(clientViewModel, new JsonApiSerializerSettings());
            
            var response = await _client.GetAsync(endPoint);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }
        
        
        [Test]
        public async Task Server_Can_Create_New_Client()
        {
            var endPoint = ServerUri + "clients/";
            var requestBody = "{\"firstname\":\"John\",\"lastname\":\"Smith\"}";
            var expectedBody = JsonConvert.SerializeObject(new IdViewModel {Id = 3}, new JsonApiSerializerSettings());
            
            var response = await _client.PostAsync(endPoint, new StringContent(requestBody));
            
            var responseBody = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }


        [Test]
        public async Task Server_Can_Delete_Client()
        {
            var endPoint = ServerUri + "clients/1";
            var expectedBody = "Deleted";
            
            var response = await _client.DeleteAsync(endPoint);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }
        

        [Test]
        public async Task Server_Can_Edit_Client()
        {
            var endPoint = ServerUri + "clients/1";
            var newFirstName = "NEW FIRST NAME";
            var newLastName = "NEW LAST NAME";
            var requestBody = $"{{\"firstname\":\"{newFirstName}\",\"lastname\":\"{newLastName}\"}}";
            var expectedBody = "Updated";
            
            var response = await _client.PutAsync(endPoint, new StringContent(requestBody));
            
            var responseBody = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedBody, responseBody);
        }
    }
}