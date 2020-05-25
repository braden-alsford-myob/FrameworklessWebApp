using System;
using System.Collections.Generic;
using System.Linq;
using FrameworklessWebApp.Data;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services.Exceptions;

namespace FrameworklessWebApp.Services
{
    public class JournalEntryService
    {
        private readonly IRetriever _retriever;


        public JournalEntryService(IRetriever retriever)
        {
            _retriever = retriever;
        }
        

        public List<JournalEntry> GetEntries(int clientId)
        {
            var entries = _retriever.GetJournalEntries(clientId);
            
            // TODO fix this up.
            // if (entries.Count == 0)
            // {
            //     throw new ClientNotFoundException(clientId);
            // }

            return entries.Result;
        }
        

        public int AddEntry(int clientId, JournalEntry journalEntry)
        {
            var entries = GetEntries(clientId);
            var newId = GetNextId(entries);
            journalEntry.Id = newId;
            journalEntry.ClientId = clientId;
            
            _retriever.AddJournalEntryAsync(journalEntry);

            return newId;
        }


        public JournalEntry GetEntryById(int clientId, int entryId)
        {
            return _retriever.GetJournalEntryAsync(clientId, entryId);
        }
        

        public void DeleteEntry(int clientId, int entryId)
        {
            var entry = GetEntryById(clientId, entryId);
            _retriever.DeleteJournalEntryAsync(clientId, entry);
        }


        public void UpdateEntry(int clientId, int entryId, JournalEntry updatedEntry)
        {
            ValidateNewJournalEntry(updatedEntry);
            
            var entryToUpdate = GetEntryById(clientId, entryId);
            entryToUpdate.Content = updatedEntry.Content;
            entryToUpdate.Date = updatedEntry.Date;
            
            _retriever.UpdateJournalEntryAsync(clientId, entryToUpdate);
        }

        private int GetNextId(List<JournalEntry> entries)
        {
            if (entries.Count == 0) return 1;
            
            var currentMaxId = entries.Max(e => e.Id);
            var nextId = currentMaxId + 1;
            
            return nextId;
        }


        private void ValidateNewJournalEntry(JournalEntry entry)
        {
            if (string.IsNullOrEmpty(entry.Content))
            {
                throw new MissingJournalEntryAttributesException("Content");
            }
            
            if (entry.Date == DateTime.MinValue)
            {
                throw new MissingJournalEntryAttributesException("DateTime");
            }
        }
    }
}