using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application
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
        

        public void AddEntry(JournalEntry journalEntry)
        {
            _idCount++;
            
            journalEntry.Id = _idCount;
            _entries.Add(journalEntry);
        }
        

        public void DeleteEntry(int entryId)
        {
            foreach (var entry in _entries.Where(entry => entry.Id == entryId))
            {
                _entries.Remove(entry);
            }
        }
    }
}