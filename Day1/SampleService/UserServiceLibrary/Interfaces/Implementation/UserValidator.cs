using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces.Implementation
{
    public class UserValidator : IUserValidator
    {
        /// <summary>
        /// Validate user to not null fields
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
