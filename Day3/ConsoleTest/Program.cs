using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var us = new User(109);
            Console.WriteLine("Console");
            us.FirstName = "12313213213444";
            us.LastName = "123213213";
            var t = us.IsValid();
            Console.WriteLine(t);
            Console.ReadKey();
        }
    }
}
