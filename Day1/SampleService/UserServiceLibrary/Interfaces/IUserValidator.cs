﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.Interfaces
{
    public interface IUserValidator
    {
        bool Validate(User user);
    }
}
