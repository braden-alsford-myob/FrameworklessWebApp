using System.Collections.Generic;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application.Models
{
    public class Client
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }

        [JsonConstructor]
        public Client(string username, string firstName, string lastName)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}