using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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
            var x = new List<string>();
            var slaves = new List<Slave>();
            var m = new Master(uss);
            for (int i = 0; i < Int32.Parse(myappSet["slavesNum"]); i++)
            {
                var temp = myappSet[$"slave{i}"].Split(' ');
                x.Add(temp[0]);
            }
            foreach (var item in x)
            {
                slaves.Add(new Slave(uss,item));
            }
            Thread.Sleep(2000);
            m.Add(validateUser);
            Thread.Sleep(2000);
            m.Search(validateUser);
            //uss.Dump();
            Console.ReadLine();
        }
    }
}
