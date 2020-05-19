using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using FrameworklessWebApp.Models;

namespace FrameworklessWebApp.Data
{
    public class DynamoRetriever : IRetriever
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        private readonly Table _journalEntryTable;
        
        private readonly string _journalTableName;
        private readonly string _clientTableName;
        

        public DynamoRetriever()
        {
            try
            {
                var clientConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
                _client = new AmazonDynamoDBClient(clientConfig);
                _context = new DynamoDBContext(_client);
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            
            _clientTableName = "Clients";
            _journalTableName = "Journals";
            
            _journalEntryTable = Table.LoadTable(_client, _journalTableName);
        }
        
        public async Task<List<Client>> GetClientsAsync()
        {
            var request = new ScanRequest
            {
                TableName = _clientTableName
            };
            
            var response =  await _client.ScanAsync(request);
            
            var clients = new List<Client>();
            foreach (var item in response.Items)
            {
                clients.Add(new Client
                {
                    ClientID = int.Parse(item["ClientID"].N),
                    FirstName = item["FirstName"].S,
                    LastName = item["LastName"].S
                });
            }
            
            return clients;
        }

        public Client GetClient(int id)
        {
            var response = _context.LoadAsync<Client>(id);
            return response.Result;
        }
        

        public async Task AddClientAsync(Client client)
        {
            await _context.SaveAsync(client);
        }
        

        public async Task DeleteClientAsync(Client client)
        {
            await _context.DeleteAsync<Client>(client.ClientID);
        }

        public async Task UpdateClientAsync(int id, Client newClient)
        {
            var oldClient = GetClient(id);

            oldClient.FirstName = newClient.FirstName;
            oldClient.LastName = newClient.LastName;

            await _context.SaveAsync(oldClient);
        }
        

        public async Task<List<JournalEntry>> GetJournalEntries(int id)
        {
            var table = Table.LoadTable(_client, _journalTableName);
            var filter = new QueryFilter("Id", QueryOperator.Equal, id.ToString());
            var search = table.Query(filter);
            
            var documentSet = new List<Document>();
            do
            {
                documentSet = await search.GetNextSetAsync();
                
            } while (!search.IsDone);


            var journalEntries = new List<JournalEntry>();

            foreach (var document in documentSet)
            {
                var rangeKey = document["Date"].ToString().Split("$");
                
                journalEntries.Add(new JournalEntry
                (
                     int.Parse(rangeKey[1]), 
                     int.Parse(document["Id"]),
                     ConvertFromUnixTimestamp(double.Parse(rangeKey[0])),
                     document["Content"]
                ));
            }

            return journalEntries;
        }

        
        public async Task AddJournalEntryAsync(JournalEntry entry)
        {
            var dateNumber = ConvertToUnixTimestamp(entry.Date);
            
            var journalEntry = new Document
            {
                ["Id"] = entry.ClientId.ToString(),
                ["Date"] = dateNumber + "$" + entry.Id,
                ["Content"] = entry.Content
            };

            await _journalEntryTable.PutItemAsync(journalEntry);
        }

        public async Task DeleteJournalEntryAsync(int clientId, JournalEntry entry)
        {
            var rangeKey = ConvertToUnixTimestamp(entry.Date) + "$" + entry.Id;
            await _journalEntryTable.DeleteItemAsync(clientId.ToString(), rangeKey);
        }

        public async Task UpdateJournalEntryAsync(int clientId, JournalEntry updatedEntry)
        {
            await DeleteJournalEntryAsync(clientId, updatedEntry);
            await AddJournalEntryAsync(updatedEntry);
        }

        public JournalEntry GetJournalEntryAsync(int clientId, int entryId)
        {
            var entries = GetJournalEntries(clientId).Result;

            foreach (var entry in entries.Where(entry => entry.Id == entryId))
            {
                return entry;
            }

            return null;
        }


        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}