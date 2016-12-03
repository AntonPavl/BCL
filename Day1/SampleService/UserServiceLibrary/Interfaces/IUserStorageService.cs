using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IUserStorageService
    {
        int Add(User user);

        IEnumerable<int> AddRange(IEnumerable<User> users);

        bool Remove(User user);

        User Search(User user);

        IEnumerable<User> SearchByPredicate(Func<User, bool> f);

        void Dump(IDump d);
    }
}
