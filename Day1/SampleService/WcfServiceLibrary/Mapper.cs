using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;

namespace WcfServiceLibrary
{
    public static class Mapper
    {
        public static User ToUser(this UserDataContract userDC)
        {
            return new User()
            {
                Id = userDC.Id,
                FirstName = userDC.FirstName,
                LastName = userDC.LastName,
                DateOfBirth = userDC.DateOfBirth,
                Gender = userDC.Gender,
                VisaRecords = userDC.VisaRecords.Select(x => x.ToVisa()).ToList()
            };
        }

        public static Visa ToVisa(this VisaDataContract visa)
        {
            return new Visa()
            {
                Country = visa.Country,
                Start = visa.Start,
                End = visa.End
            };
        }
    }
}
