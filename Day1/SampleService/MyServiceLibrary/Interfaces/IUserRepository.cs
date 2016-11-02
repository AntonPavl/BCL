using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IUserRepository 
    {
        User Add(User user);

        IEnumerable<User> AddRange(IEnumerable<User> users);

        bool Remove(User user);

        User Search(User user);

        IEnumerable<User> SearchByPredicate(Func<User,User> f);

        bool Contains(User user);
    }
}
