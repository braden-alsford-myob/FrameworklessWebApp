using System.Collections.Generic;

namespace FrameworklessWebApp.Models
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static ClientViewModel ConvertToViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.ClientID,
                FirstName = client.FirstName,
                LastName = client.LastName
            };
        }
    }
}