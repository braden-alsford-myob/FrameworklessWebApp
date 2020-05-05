using System;

namespace FrameworklessWebApp.Application.Exceptions
{
    public class NameTakenException : Exception
    {
        public NameTakenException(string firstName, string lastName): base(FormatMessage(firstName, lastName))
        {
        }

        private static string FormatMessage(string firstName, string lastName)
        {
            return $"{firstName} {lastName} is already taken!";
        }
    }
}