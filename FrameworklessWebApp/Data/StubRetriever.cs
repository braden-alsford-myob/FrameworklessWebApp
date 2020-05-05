using System;
using System.Collections.Generic;
using FrameworklessWebApp.Application;
using FrameworklessWebApp.Application.Models;

namespace FrameworklessWebApp.Database
{
    public class StubRetriever : IRetriever
    {
        private List<Client> _clients;

        public StubRetriever()
        {
            _clients = new List<Client>
            {
                new Client(
                    "Braden", 
                    "Alsford", 
                    new List<JournalEntry>
                    {
                        new JournalEntry(new DateTime(2020, 12, 1), "Braden's first entry")
                    })
            };
        }
        
        public List<Client> GetClients()
        {
            return _clients;
        }

        public void AddClient(Client client)
        {
            _clients.Add(client);
        }
    }
}