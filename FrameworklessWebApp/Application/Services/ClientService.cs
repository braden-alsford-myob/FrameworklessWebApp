using System.Collections.Generic;
using System.Linq;
using FrameworklessWebApp.Application.Exceptions;
using FrameworklessWebApp.Database;

namespace FrameworklessWebApp.Application.Services
{
    public class ClientService
    {
        private readonly IRetriever _retriever;

        public ClientService(IRetriever retriever)
        {
            _retriever = retriever;
        }

        public List<Client> GetClients()
        {
            return _retriever.GetClients();
        }

        public void AddClient(Client client)
        {
            if (ClientNameTaken(client))
            {
                throw new NameTakenException(client.FirstName, client.LastName);
            }
            
            _retriever.AddClient(client);
        }

        private bool ClientNameTaken(Client newClient)
        {
            return GetClients().Any(client => client.FirstName == newClient.FirstName && client.LastName == newClient.LastName);
        }
    }
}