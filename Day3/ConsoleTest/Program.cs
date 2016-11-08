using Attributes;
using Attributes.Interfaces.Implementations;
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
            var users = new List<User>();
            var advUsers = new List<AdvancedUser>();
            var uv = new UserValidator();
            var ass = typeof(User).Assembly;

            var attrUsers = (InstantiateUserAttribute[])typeof(User).GetCustomAttributes(typeof(InstantiateUserAttribute), false);
            foreach (var user in attrUsers)
            {
                users.Add(new User(user.Id) { FirstName = user.FirstName, LastName = user.LastName });
            }

            var assUsers = (InstantiateAdvancedUserAttribute[])Attribute.GetCustomAttributes(ass,typeof(InstantiateAdvancedUserAttribute), false);
            foreach (var user in assUsers)
            {
                advUsers.Add(new AdvancedUser(user.Id,user.ExternalId) { FirstName = user.FirstName, LastName = user.LastName });
            }

            Console.ReadKey();
        }
    }
}
