using System.Collections.Generic;
using System.Linq;
using FrameworklessWebApp.Data;
using FrameworklessWebApp.Models;
using FrameworklessWebApp.Services.Exceptions;

namespace FrameworklessWebApp.Services
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
        
        
        public Client GetClientById(int id)
        {
            foreach (var client in GetClients().Where(client => client.ClientID == id))
            {
                return client;
            }

            throw new ClientNotFoundException(id);
        }

        
        public int AddClient(Client client)
        {
            var clients = GetClients();
            var id = GetNextId(clients);
            client.ClientID = id;
            
            _retriever.AddClient(client);

            return id;
        }

        
        public void DeleteClient(int id)
        {
            var clientToDelete = GetClientById(id);
            _retriever.DeleteClient(clientToDelete);
        }


        public void UpdateClient(int id, Client newClient)
        {
            _retriever.UpdateClient(id, newClient);
        }
        
        private int GetNextId(List<Client> clients)
        {
            if (clients.Count == 0)
            {
                return 1;
            }
            
            var currentMaxId = clients.Max(c => c.ClientID);
            return currentMaxId + 1;
        }
    }
}