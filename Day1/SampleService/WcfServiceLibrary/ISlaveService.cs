﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary;

namespace WcfServiceLibrary
{
    [ServiceContract]
    public interface ISlaveService
    {

        [OperationContract]
        IEnumerable<User> Search(UserDataContract user);
    }
}
