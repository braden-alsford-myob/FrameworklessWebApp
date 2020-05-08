using System;

namespace FrameworklessWebApp.Services.Exceptions
{
    public class MissingJournalEntryAttributesException : Exception
    {
        public MissingJournalEntryAttributesException(string missingAttribute) : base(FormatMessage(missingAttribute))
        {
            
        }

        private static string FormatMessage(string missingAttribute)
        {
            return $"{missingAttribute} is missing from the body of the PUT.";
        }
    }
}