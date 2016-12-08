using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;
using UserServiceLibrary.Interfaces.Implementation;
using UserServiceLibrary.Interfaces.Implementations;

namespace SampleService
{
    class Program
    {
        static void Main(string[] args)
        {
            //MasterSlaveCreator.Create();
            var validateUser = new User() { FirstName = "anton", LastName = "Pavlenok", DateOfBirth = DateTime.Today, Gender = Gender.Male, VisaRecords = new List<Visa>() };
            var appSettings = ConfigurationManager.AppSettings;
            var uss = new UserStorageService(new UserRepository());
            var myappSet = (dynamic)ConfigurationManager.GetSection("Slaves");
            for (int i = 0; i < Int32.Parse(myappSet["slavesNum"]); i++)
            {
                var x = myappSet[$"slave{i}"].Split(' ');
                Console.WriteLine(x[0]);
                Console.WriteLine(x[1]);
            }
            //var s = new Slave(uss);
            //var ss = new Slave(uss);
            //s.Listen();
            //var m = new Master(uss);
            // m.Add(validateUser);
            //uss.Dump();
            //Console.ReadLine();
        }
    }
}
