using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary
{
    public struct VisaRecord
    {
        public string Country { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
