using MyOwnDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string name;
        public string lastName;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            User Us = new User { name = "Гриша", lastName = "Федоров", Id = Guid.NewGuid()};

            Console.WriteLine(Us.Id == Guid.Empty);
        }
    }
}
