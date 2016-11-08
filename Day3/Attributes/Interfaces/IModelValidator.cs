using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes.Interfaces
{
    public interface IModelValidator<T>
    {
        bool IsValid(T t);
    }
}
