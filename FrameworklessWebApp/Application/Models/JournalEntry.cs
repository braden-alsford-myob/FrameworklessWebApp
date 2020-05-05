using System;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        
        public DateTime TimeAdded { get; set; }
        
        public string Content { get; set; }
        
        
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