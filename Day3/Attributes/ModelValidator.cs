using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    public interface ModelValidator<T>
    {
        bool IsValid(T t);
    }
}
