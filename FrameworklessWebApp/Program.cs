using System;
using System.Collections.Generic;
using System.Threading;
using FrameworklessWebApp.API;
using FrameworklessWebApp.Application;

namespace FrameworklessWebApp
{
    class Program
    {
        private const string Port = "8080";
        private static readonly string Uri = $"http://localhost:{Port}/";


        static void Main(string[] args)
        {
            var initialJournalEntries = new List<JournalEntry>
            {
                new JournalEntry(1, DateTime.Now, "This entry went in today!"),
                new JournalEntry(2, new DateTime(2020, 4, 29), "This one went in yesterday :)")
            };

            var journalEntryService = new JournalEntryService(initialJournalEntries);



            var router = new Router(journalEntryService);

            var server = new Server(Uri, router);

            Console.WriteLine($"Server listening on port: {Port}");
            server.Run();
        }
    }
}