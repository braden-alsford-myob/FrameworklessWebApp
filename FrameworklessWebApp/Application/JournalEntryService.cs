using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application
{
    public class JournalEntryService
    {
        private readonly List<JournalEntry> _entries;

        public JournalEntryService(List<JournalEntry> entries)
        {
            _entries = entries;
        }
        

        public List<JournalEntry> GetEntries()
        {
            return _entries;
        }
        

        public void AddEntry(JournalEntry journalEntry)
        {
            if (!EntryAlreadyExists(journalEntry.Id))
            {
                _entries.Add(journalEntry);
            }
            
            // todo throw exception if already exists.
        }
        

        public void DeleteEntry(int entryId)
        {
            foreach (var entry in _entries.Where(entry => entry.Id == entryId))
            {
                _entries.Remove(entry);
            }
        }

        private bool EntryAlreadyExists(int entryId)
        {
            return _entries.Any(entry => entry.Id == entryId);
        }
    }
}