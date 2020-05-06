using System;

namespace FrameworklessWebApp.Application.Exceptions
{
    public class NameTakenException : Exception
    {
        public NameTakenException(string username): base(FormatMessage(username))
        {
        }

        private static string FormatMessage(string username)
        {
            return $"{username} is already taken!";
        }
    }
}