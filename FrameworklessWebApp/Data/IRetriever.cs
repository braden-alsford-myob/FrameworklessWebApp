using System.Collections.Generic;
using FrameworklessWebApp.Application;

namespace FrameworklessWebApp.Database
{
    public interface IRetriever
    {
        List<Client> GetClients();
        void AddClient(Client client);
    }
}