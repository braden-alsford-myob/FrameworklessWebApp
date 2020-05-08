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
        

        public List<JournalEntry> GetEntries(string username)
        {
            var entries = _retriever.GetJournalEntries(username);
            if (entries.Count == 0)
            {
                throw new ClientNotFoundException(username);
            }

            return entries;
        }
        

        public int AddEntry(string username, JournalEntry journalEntry)
        {
            var entries = GetEntries(username);
            var id = GetNextId(entries);
            
            journalEntry.Id = id;
            
            _retriever.AddJournalEntry(username, journalEntry);

            return id;
        }


        public JournalEntry GetEntryById(string username, int id)
        {
            foreach (var entry in GetEntries(username).Where(entry => entry.Id == id))
            {
                return entry;
            }

            throw new JournalEntryNotFoundException(id);
        }
        

        public void DeleteEntry(string username, int entryId)
        {
            var entry = GetEntryById(username, entryId);
            _retriever.DeleteJournalEntry(username, entry);
        }


        public void UpdateEntry(string username, int entryId, JournalEntry updatedEntry)
        {
            ValidateNewJournalEntry(updatedEntry);
            
            var entryToUpdate = GetEntryById(username, entryId);
            entryToUpdate.Content = updatedEntry.Content;
            entryToUpdate.TimeAdded = updatedEntry.TimeAdded;
            
            _retriever.UpdateJournalEntry(username, entryToUpdate);
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