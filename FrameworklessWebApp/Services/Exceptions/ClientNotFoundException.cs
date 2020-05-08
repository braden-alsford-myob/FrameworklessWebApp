using System;

namespace FrameworklessWebApp.Services.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(int id) : base(FormatMessage(id))
        {
            
        }

        private static string FormatMessage(int id)
        {
            return $"No client with the id '{id}' was found.";
        }
    }
}