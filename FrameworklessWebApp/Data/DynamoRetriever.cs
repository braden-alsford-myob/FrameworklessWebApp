using System;
using System.Collections.Generic;
using System.Globalization;
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
        
        private readonly string _tableName;
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
            
            _tableName = "JournalManager";
            _clientTableName = "Clients";
            _journalTableName = "Journals";
        }

        public List<Client> GetClients()
        {
            // var clients = _context.ScanAsync<Client>(new List<ScanCondition>());

            var request = new ScanRequest
            {
                TableName = _clientTableName
            };
            
            var response = Task.Run(() => _client.ScanAsync(request));
            
            var clients = new List<Client>();
            foreach (var item in response.Result.Items)
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
        

        public void AddClient(Client client)
        {
            _context.SaveAsync(client);
        }
        

        public void DeleteClient(Client client)
        {
            _context.DeleteAsync<Client>(client.ClientID);
        }

        public void UpdateClient(int id, Client newClient)
        {
            var oldClient = GetClient(id);

            oldClient.FirstName = newClient.FirstName;
            oldClient.LastName = newClient.LastName;
            oldClient.JournalEntries = newClient.JournalEntries;

            _context.SaveAsync(oldClient);
        }

        public List<JournalEntry> GetJournalEntries(int id)
        {
            var request = new QueryRequest()
            {
                TableName = _journalTableName,
                KeyConditionExpression = "ClientID = :v_ClientID",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {
                        ":v_ClientID", new AttributeValue { N =  id.ToString() }
                    }},
                ConsistentRead = true
            };
            var response = Task.Run(() =>_client.QueryAsync(request));

            var entries = new List<JournalEntry>();
            foreach (var item in response.Result.Items)
            {
                entries.Add(new JournalEntry
                (
                    int.Parse(item["ClientID"].N),
                    ConvertFromUnixTimestamp(double.Parse(item["Date"].N)),
                    item["Content"].S
                ));
            }

            return entries;
        }

        public void AddJournalEntry(int id, JournalEntry entry)
        {
            var request = new PutItemRequest
            {
                TableName = _journalTableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "ClientID", new AttributeValue {
                        N = id.ToString()
                    }},
                    { "Date", new AttributeValue
                    {
                        N = ConvertToUnixTimestamp(entry.TimeAdded.Date).ToString(CultureInfo.InvariantCulture)
                    }},
                    { "Content", new AttributeValue
                    {
                        S = entry.Content
                    }}
                }
            };
            
            var response = Task.Run(() =>_client.PutItemAsync(request));
        }

        public void DeleteJournalEntry(int id, JournalEntry entry)
        {
            var request = new DeleteItemRequest
            {
                TableName = _clientTableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "ClientID", new AttributeValue {
                        N = id.ToString()
                    }},
                    {"Date", new AttributeValue
                    {
                        N = ConvertToUnixTimestamp(entry.TimeAdded.Date).ToString(CultureInfo.InvariantCulture)
                    }}
                }
            };

            var response = Task.Run(() => _client.DeleteItemAsync(request));
        }

        public void UpdateJournalEntry(int id, JournalEntry updatedEntry)
        {
            throw new System.NotImplementedException();
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