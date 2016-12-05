using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;
using UserServiceLibrary.Interfaces.Generic;
using UserServiceLibrary.Interfaces.Implementations;

namespace WcfServiceLibrary
{
    public class MasterService : IMasterService
    {
        private static readonly Master master;
        static MasterService()
        {
            IService<User> s = new UserStorageService();
            master = new Master(s);
        }
        public int Add(UserDataContract user) => master.Add(user.ToUser());

        public bool Remove(UserDataContract user) => master.Remove(user.ToUser());
        public IEnumerable<User> Search(UserDataContract user) => master.Search(user.ToUser());
    }
}
