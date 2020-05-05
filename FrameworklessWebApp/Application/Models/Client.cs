using System.Collections.Generic;
using FrameworklessWebApp.Application.Models;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }

        [JsonConstructor]
        public Client(string firstName, string lastName, List<JournalEntry> journalEntries)
        {
            FirstName = firstName;
            LastName = lastName;
            JournalEntries = journalEntries;
        }
    }
}