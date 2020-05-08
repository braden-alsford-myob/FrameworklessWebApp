using System.Collections.Generic;
using FrameworklessWebApp.Models;

namespace FrameworklessWebApp.Data
{
    public interface IRetriever
    {
        List<Client> GetClients();
        void AddClient(Client client);
        void DeleteClient(Client client);
        void UpdateClient(string oldUsername, Client newClient);
        List<JournalEntry> GetJournalEntries(string username);
        void AddJournalEntry(string username, JournalEntry entry);
        void DeleteJournalEntry(string username, JournalEntry entry);
        void UpdateJournalEntry(string username, JournalEntry updatedEntry);
    }
}