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
        
        
        public Client GetClientByUsername(string username)
        {
            foreach (var client in GetClients().Where(client => client.Username == username))
            {
                return client;
            }

            throw new ClientNotFoundException(username);
        }

        
        public void AddClient(Client client)
        {
            if (ClientUsernameTaken(client.Username))
            {
                throw new NameTakenException(client.Username);
            }
            
            _retriever.AddClient(client);
        }

        
        public void DeleteClient(string username)
        {
            var clientToDelete = GetClientByUsername(username);
            _retriever.DeleteClient(clientToDelete);
        }


        public void UpdateClient(string oldUsername, Client newClient)
        {
            _retriever.UpdateClient(oldUsername, newClient);
        }


        private bool ClientUsernameTaken(string username)
        {
            return GetClients().Any(client => client.Username == username);
        }
    }
}