using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces.Implementation
{
    public class FirstNameSearchCriteria : ISearchCriteria
    {
        private string data;
        public FirstNameSearchCriteria(string data)
        {
            this.data = data;
        }
        public bool Search(User item)
        {
            return item.FirstName.Equals(data);
        }
    }
}
