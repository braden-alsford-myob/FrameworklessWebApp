using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Models
{
    [DynamoDBTable("Clients")]
    public class Client
    {
        [DynamoDBHashKey]
        public int ClientID { get; set; }
        
        [DynamoDBProperty]
        public string FirstName { get; set; }
        
        [DynamoDBProperty]
        public string LastName { get; set; }
        
        [DynamoDBProperty]
        public List<JournalEntry> JournalEntries { get; set; }

        [JsonConstructor]
        public Client(int clientId, string firstName, string lastName, List<JournalEntry> journalEntries)
        {
            FirstName = firstName;
            LastName = lastName;
            ClientID = clientId;
            JournalEntries = journalEntries;
        }

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            ClientID = 0;
        }
        
        public Client() {}


        public static Client ConvertToClient(ClientViewModel clientVm)
        {
            return new Client(clientVm.FirstName, clientVm.LastName)
            {
                ClientID = clientVm.Id,
                JournalEntries = clientVm.JournalEntries
            };
        }
    }
}