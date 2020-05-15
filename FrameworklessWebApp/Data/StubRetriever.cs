using System.Collections.Generic;
using System.Linq;
using FrameworklessWebApp.Models;

namespace FrameworklessWebApp.Data
{
    public class StubRetriever : IRetriever
    {
        private readonly List<Client> _clients;

        public StubRetriever(List<Client> clients)
        {
            _clients = clients;
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

        
        public void UpdateClient(int id, Client newClient)
        {
            foreach (var client in _clients.Where(client => client.ClientID == id))
            {
                client.FirstName = newClient.FirstName;
                client.LastName = newClient.LastName;
            }
        }


        public List<JournalEntry> GetJournalEntries(int id)
        {
            foreach (var client in _clients.Where(client => client.ClientID == id))
            {
                return client.JournalEntries;
            }
            
            return new List<JournalEntry>();
        }

        
        public void AddJournalEntry(int id, JournalEntry entry)
        {
            foreach (var client in _clients.Where(client => client.ClientID == id))
            {
                client.JournalEntries.Add(entry);
            }
        }

        public void DeleteJournalEntry(int id, JournalEntry entry)
        {
            foreach (var client in _clients.Where(client => client.ClientID == id))
            {
                client.JournalEntries.Remove(entry);
            }
        }

        
        public void UpdateJournalEntry(int id, JournalEntry updatedEntry)
        {
            foreach (var client in _clients.Where(client => client.ClientID == id))
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