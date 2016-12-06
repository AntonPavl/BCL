using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IDumper<T>
    {
        void Dump(IEnumerable<T> list, string path = null);

        IEnumerable<T> GetDump(string path = null);
    }
}
