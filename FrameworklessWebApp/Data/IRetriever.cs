using System.Collections.Generic;
using System.Threading.Tasks;
using FrameworklessWebApp.Models;

namespace FrameworklessWebApp.Data
{
    public interface IRetriever
    {
        Task<List<Client>> GetClientsAsync();
        Task AddClientAsync(Client client);
        Task DeleteClientAsync(Client client);
        Task UpdateClientAsync(int id, Client newClient);
        Task<List<JournalEntry>> GetJournalEntries(int id);
        Task AddJournalEntryAsync(JournalEntry entry);
        Task DeleteJournalEntryAsync(int clientId, JournalEntry entry);
        Task UpdateJournalEntryAsync(int clientId, JournalEntry updatedEntry);
        JournalEntry GetJournalEntryAsync(int clientId, int entryId);
    }
}