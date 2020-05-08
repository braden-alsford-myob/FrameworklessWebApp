using System;
using FrameworklessWebApp.Application.Models;

namespace FrameworklessWebApp.API.ViewModels
{
    public class JournalEntryViewModel
    {
        public int Id { get; set; }
        public DateTime TimeAdded { get; set; }
        public string Content { get; set; }

        
        public static JournalEntryViewModel ConvertToViewModel(JournalEntry entry)
        {
            return new JournalEntryViewModel
            {
                Id = entry.Id,
                TimeAdded = entry.TimeAdded,
                Content = entry.Content
            };
        }
    }
}