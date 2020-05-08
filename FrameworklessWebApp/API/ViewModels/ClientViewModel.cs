using System.Collections.Generic;
using FrameworklessWebApp.Application.Models;

namespace FrameworklessWebApp.API.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }


        public static ClientViewModel ConvertToViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                Username = client.Username,
                FirstName = client.FirstName,
                LastName = client.LastName,
                JournalEntries = client.JournalEntries
            };
        }
    }
}