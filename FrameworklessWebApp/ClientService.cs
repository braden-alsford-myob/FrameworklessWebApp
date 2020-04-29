using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FrameworklessWebApp
{
    public class ClientService
    {
        private readonly List<Client> _clients;

        public ClientService(List<Client> clients)
        {
            _clients = clients;
        }
        

        public string GetClients()
        {
            return JsonConvert.SerializeObject(_clients);
        }
        

        public void AddClient(Client client)
        {
            if (!ClientAlreadyExists(client.Name))
            {
                _clients.Add(client);
            }
            
            // todo throw exception if already exists.
        }
        

        public void DeleteClient(string name)
        {
            foreach (var client in _clients.Where(client => client.Name == name))
            {
                _clients.Remove(client);
            }
        }

        private bool ClientAlreadyExists(string name)
        {
            return _clients.Any(client => client.Name == name);
        }
    }
}