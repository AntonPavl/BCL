using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces.Generic
{
    public interface IService<T>
    {
        int Add(T user);

        IEnumerable<int> AddRange(IEnumerable<T> users);

        bool Remove(T user);

        bool Contains(T user);

        User Search(T user);

        IEnumerable<T> SearchByPredicate(Func<T, bool> f);

        IEnumerable<T> GetEntities();

        T GetEntityById(int i);
    }
}
