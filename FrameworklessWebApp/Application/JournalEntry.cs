using System;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application
{
    public class JournalEntry
    {
        public int Id { get; set; }
        
        public DateTime TimeAdded { get; }
        
        public string Content { get; }
        
        
        [JsonConstructor]
        public JournalEntry(DateTime timeAdded, string content)
        {
            TimeAdded = timeAdded;
            Content = content;
        }        
        
        
        public JournalEntry(int id, DateTime timeAdded, string content)
        {
            Id = id;
            TimeAdded = timeAdded;
            Content = content;
        }
    }
}