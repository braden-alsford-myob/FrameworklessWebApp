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
            if (entries.Count == 0)
            {
                throw new ClientNotFoundException(clientId);
            }

            return entries;
        }
        

        public int AddEntry(int clientId, JournalEntry journalEntry)
        {
            var entries = GetEntries(clientId);
            var id = GetNextId(entries);
            journalEntry.Id = id;
            
            _retriever.AddJournalEntry(clientId, journalEntry);

            return id;
        }


        public JournalEntry GetEntryById(int clientId, int entryId)
        {
            foreach (var entry in GetEntries(clientId).Where(entry => entry.Id == entryId))
            {
                return entry;
            }

            throw new JournalEntryNotFoundException(entryId);
        }
        

        public void DeleteEntry(int clientId, int entryId)
        {
            var entry = GetEntryById(clientId, entryId);
            _retriever.DeleteJournalEntry(clientId, entry);
        }


        public void UpdateEntry(int clientId, int entryId, JournalEntry updatedEntry)
        {
            ValidateNewJournalEntry(updatedEntry);
            
            var entryToUpdate = GetEntryById(clientId, entryId);
            entryToUpdate.Content = updatedEntry.Content;
            entryToUpdate.TimeAdded = updatedEntry.TimeAdded;
            
            _retriever.UpdateJournalEntry(clientId, entryToUpdate);
        }

        private int GetNextId(List<JournalEntry> entries)
        {
            var currentMaxId = entries.Max(e => e.Id);
            
            return currentMaxId + 1;
        }


        private void ValidateNewJournalEntry(JournalEntry entry)
        {
            if (string.IsNullOrEmpty(entry.Content))
            {
                throw new MissingJournalEntryAttributesException("Content");
            }
            
            if (entry.TimeAdded == DateTime.MinValue)
            {
                throw new MissingJournalEntryAttributesException("DateTime");
            }
        }
    }
}