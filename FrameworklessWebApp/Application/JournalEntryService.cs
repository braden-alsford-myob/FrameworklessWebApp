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
    }
}