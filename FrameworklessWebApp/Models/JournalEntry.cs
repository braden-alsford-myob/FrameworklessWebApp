using System;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Models
{
    public class JournalEntry
    {
        // Partition key eg. {clientId}
        // Sort key eg. {date}${journalId}
        
        public int Id { get; set; }
        public int ClientId { get; set; }    
        public DateTime Date { get; set; }
        public string Content { get; set; }
        
        
        [JsonConstructor]
        public JournalEntry(DateTime date, string content)
        {
            Date = date;
            Content = content;
        }
        
        
        public JournalEntry(int id, int clientId, DateTime date, string content)
        {
            Id = id;
            ClientId = clientId;
            Date = date;
            Content = content;
        }


        public static JournalEntry ConvertToJournalEntry(JournalEntryViewModel entryVm)
        {
            return new JournalEntry(entryVm.Id, entryVm.ClientId, entryVm.Date, entryVm.Content);
        }
    }
}