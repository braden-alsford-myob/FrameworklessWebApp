using System;

namespace FrameworklessWebApp.Services.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(string username) : base(FormatMessage(username))
        {
            
        }

        private static string FormatMessage(string username)
        {
            return $"No client with the username '{username}' was found.";
        }
    }
}