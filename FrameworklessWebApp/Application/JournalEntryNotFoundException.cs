using System;

namespace FrameworklessWebApp.Application
{
    public class JournalEntryNotFoundException : Exception
    {
        public JournalEntryNotFoundException(int id) : base(FormatMessage(id))
        {
            
        }

        private static string FormatMessage(in int id)
        {
            return $"No Journal Entry with an id of {id} was found.";
        }
    }
}