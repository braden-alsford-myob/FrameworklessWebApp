using System;

namespace FrameworklessWebApp.Application
{
    public class JournalEntry
    {
        public int Id { get; }
        public DateTime DateTime { get; }
        public string Content { get; }
        
        
        public JournalEntry(int id, DateTime dateTime, string content)
        {
            Id = id;
            DateTime = dateTime;
            Content = content;
        }
    }
}