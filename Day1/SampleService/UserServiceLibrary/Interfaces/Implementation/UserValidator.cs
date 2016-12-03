using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces.Implementation
{
    public class UserValidator : IUserValidator
    {
        public bool Validate(User user)
        {
            if (user.FirstName == null ||
                user.LastName == null ||
                user.DateOfBirth == null ||
                user.VisaRecords == null) return false;
            return true;
        }

    }
}
