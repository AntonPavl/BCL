using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserServiceLibrary;

namespace WcfServiceLibrary
{
    public class SlaveService : ISlaveService
    {
        public static Slave slave;

        public static object ConfigurationManager { get; private set; }

        static SlaveService()
        {
            Init();
        }

        private static void Init()
        {
            CreateSlave(5);
        }

        private static void CreateSlave(int num)
        {

            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Slave")
            };
            for (int i = 0; i < num; i++)
            {
                var domain = AppDomain.CreateDomain("Slave" + i, null, appDomainSetup);


                slave = (Slave)domain.CreateInstanceAndUnwrap("MasterSlaveReplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(Slave).FullName);
                new Thread(() => slave.Listen()).Start();
            }

        }

        public IEnumerable<User> Search(UserDataContract user)
        {
            return slave.Search(user.ToUser());
        }
    }
}
