using System.Collections.Generic;
using FrameworklessWebApp.API.ViewModels;
using Newtonsoft.Json;

namespace FrameworklessWebApp.Application.Models
{
    public class Client
    {
        public int Id { get; set; }
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
            Id = 0;
        }

        public static Client ConvertToClient(ClientViewModel clientVm)
        {
            return new Client(clientVm.Username, clientVm.FirstName, clientVm.LastName)
            {
                Id = clientVm.Id,
                JournalEntries = clientVm.JournalEntries
            };
        }
    }
}