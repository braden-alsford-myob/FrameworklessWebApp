using System.Collections.Generic;
using FrameworklessWebApp.Models;

namespace FrameworklessWebApp.Data
{
    public interface IRetriever
    {
        List<Client> GetClients();
        void AddClient(Client client);
        void DeleteClient(Client client);
        void UpdateClient(int id, Client newClient);
        List<JournalEntry> GetJournalEntries(int id);
        void AddJournalEntry(int id, JournalEntry entry);
        void DeleteJournalEntry(int id, JournalEntry entry);
        void UpdateJournalEntry(int id, JournalEntry updatedEntry);
    }
}