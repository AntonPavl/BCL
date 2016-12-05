using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces.Generic
{
    public interface ISearchCriteria<T>
    {
        bool Search(T item);
    }
}
