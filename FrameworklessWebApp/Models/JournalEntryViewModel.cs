using System;

namespace FrameworklessWebApp.Models
{
    public class JournalEntryViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }    
        public DateTime Date { get; set; }
        public string Content { get; set; }

        
        public static JournalEntryViewModel ConvertToViewModel(JournalEntry entry)
        {
            return new JournalEntryViewModel
            {
                Id = entry.Id,
                ClientId = entry.ClientId,
                Date = entry.Date,
                Content = entry.Content
            };
        }
    }
}