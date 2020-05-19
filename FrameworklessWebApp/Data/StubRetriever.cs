using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrameworklessWebApp.Models;

namespace FrameworklessWebApp.Data
{
    public class StubRetriever : IRetriever
    {
        private readonly List<Client> _clients;
        private readonly List<JournalEntry> _journalEntries;

        public StubRetriever(List<Client> clients, List<JournalEntry> journalEntries)
        {
            _clients = clients;
            _journalEntries = journalEntries;
        }


        //region clients
        
        public async Task<List<Client>> GetClientsAsync()
        {
            return _clients;
        }

        public async Task AddClientAsync(Client client)
        {
            _clients.Add(client);
        }

        public async Task DeleteClientAsync(Client client)
        {
            _clients.Remove(client);
        }

        public async Task UpdateClientAsync(int id, Client newClient)
        {
            foreach (var client in _clients.Where(client => client.ClientID == id))
            {
                client.FirstName = newClient.FirstName;
                client.LastName = newClient.LastName;
            }
        }
        
        //end region
        
        
        //region Journal entries

        public async Task<List<JournalEntry>> GetJournalEntries(int id)
        {
            return _journalEntries.Where(j => j.ClientId == id).ToList();
        }

        public async Task AddJournalEntryAsync(JournalEntry entry)
        {
            _journalEntries.Add(entry);
        }

        public async Task DeleteJournalEntryAsync(int clientId, JournalEntry entry)
        {
            _journalEntries.Remove(entry);
        }

        public async Task UpdateJournalEntryAsync(int clientId, JournalEntry updatedEntry)
        {
            foreach (var journalEntry in _journalEntries.Where(j => j.Id == updatedEntry.Id && j.ClientId == clientId))
            {
                journalEntry.Content = updatedEntry.Content;
            }
        }

        public JournalEntry GetJournalEntryAsync(int clientId, int entryId)
        {
            return _journalEntries.FirstOrDefault(j => j.Id == entryId && j.ClientId == clientId);
        }

        // end region
    }
}