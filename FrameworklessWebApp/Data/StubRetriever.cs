using System;
using System.Collections.Generic;
using System.Linq;
using FrameworklessWebApp.Application.Models;

namespace FrameworklessWebApp.Data
{
    public class StubRetriever : IRetriever
    {
        private List<Client> _clients;

        public StubRetriever()
        {
            var client1 = new Client("bradenalsford", "Braden", "Alsford")
            {
                JournalEntries = new List<JournalEntry>
                {
                    new JournalEntry(new DateTime(2020, 12, 1), "Braden's first entry")
                }
            };

            _clients = new List<Client> { client1 };
        }
        
        
        public List<Client> GetClients()
        {
            return _clients;
        }

        
        public void AddClient(Client client)
        {
            _clients.Add(client);
        }

        
        public void DeleteClient(Client client)
        {
            _clients.Remove(client);
        }

        
        public void UpdateClient(string oldUsername, Client newClient)
        {
            foreach (var client in _clients.Where(client => client.Username == oldUsername))
            {
                client.FirstName = newClient.FirstName;
                client.LastName = newClient.LastName;
                client.Username = newClient.Username;
            }
        }


        public List<JournalEntry> GetJournalEntries(string username)
        {
            foreach (var client in _clients.Where(client => client.Username == username))
            {
                return client.JournalEntries;
            }
            
            return new List<JournalEntry>();
        }

        
        public void AddJournalEntry(string username, JournalEntry entry)
        {
            foreach (var client in _clients.Where(client => client.Username == username))
            {
                client.JournalEntries.Add(entry);
            }
        }

        public void DeleteJournalEntry(string username, JournalEntry entry)
        {
            foreach (var client in _clients.Where(client => client.Username == username))
            {
                client.JournalEntries.Remove(entry);
            }
        }

        
        public void UpdateJournalEntry(string username, JournalEntry updatedEntry)
        {
            foreach (var client in _clients.Where(client => client.Username == username))
            {
                for (var i = 0; i < client.JournalEntries.Count; i++)
                {
                    var entry = client.JournalEntries[i];

                    if (entry.Id != updatedEntry.Id) continue;
                    
                    client.JournalEntries.Remove(entry);
                    client.JournalEntries.Add(updatedEntry);
                }
            }
        }
    }
}