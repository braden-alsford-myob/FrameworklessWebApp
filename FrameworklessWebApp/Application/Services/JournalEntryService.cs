using System;
using System.Collections.Generic;
using System.Linq;
using FrameworklessWebApp.Application.Exceptions;
using FrameworklessWebApp.Application.Models;

namespace FrameworklessWebApp.Application.Services
{
    public class JournalEntryService
    {
        private readonly List<JournalEntry> _entries;
        private int _idCount;

        public JournalEntryService(List<JournalEntry> entries)
        {
            _entries = entries;
            _idCount = entries.Max(e => e.Id);
        }
        

        public List<JournalEntry> GetEntries()
        {
            return _entries;
        }
        

        public int AddEntry(JournalEntry journalEntry)
        {
            _idCount++;
            
            journalEntry.Id = _idCount;
            _entries.Add(journalEntry);

            return _idCount;
        }


        public JournalEntry GetEntryById(int id)
        {
            foreach (var entry in _entries.Where(entry => entry.Id == id))
            {
                return entry;
            }

            throw new JournalEntryNotFoundException(id);
        }
        

        public void DeleteEntry(int entryId)
        {
            var entry = GetEntryById(entryId);
            _entries.Remove(entry);
        }


        public void UpdateEntry(JournalEntry updatedEntry, int id)
        {
            ValidateNewJournalEntry(updatedEntry);
            
            var entryToUpdate = GetEntryById(id);
            entryToUpdate.Content = updatedEntry.Content;
            entryToUpdate.TimeAdded = updatedEntry.TimeAdded;
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