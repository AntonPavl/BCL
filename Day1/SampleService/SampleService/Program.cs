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
            var appSettings = ConfigurationManager.AppSettings;
            var u = new User();
            var uss = new UserStorageService(new UserRepository());
            uss.Add(new User() {FirstName="anton",LastName="Pavlenok",DateOfBirth = DateTime.Today,Gender = Gender.Male,VisaRecords = new List<Visa>()});


            var s = new Slave(uss);
            s.Listen();
            var m = new Master(uss);

            Console.WriteLine(uss.SearchByPredicate(new FirstNameSearchCriteria("anton")).ToList()[0]?.FirstName);
            uss.Dump();
            Console.Read();
        }
    }
}
