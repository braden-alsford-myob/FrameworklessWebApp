using System.Collections.Generic;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }

        [JsonConstructor]
        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = 0;
        }

        public static Client ConvertToClient(ClientViewModel clientVm)
        {
            return new Client(clientVm.FirstName, clientVm.LastName)
            {
                Id = clientVm.Id,
                JournalEntries = clientVm.JournalEntries
            };
        }
    }
}