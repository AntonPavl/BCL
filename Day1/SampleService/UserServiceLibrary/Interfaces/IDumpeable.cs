using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IDumpeable<T>
    {
        void Dump(IDumper<T> d);
        IEnumerable<T> GetEntitiesFromDump(IDumper<T> d);
    }
}
