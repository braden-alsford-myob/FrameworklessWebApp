using System.Collections.Generic;

namespace FrameworklessWebApp
{
    public class Client
    { 
        public string Name { get; }
        public int Age { get; }
        public List<string> Hobbies { get; }

        public Client(string name, int age, List<string> hobbies)
        {
            Name = name;
            Age = age;
            Hobbies = hobbies;
        }
    }
}