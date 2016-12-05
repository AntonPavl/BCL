using UserServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary
{
    [ServiceContract]
    public interface IMasterService
    {
        [OperationContract]
        int Add(UserDataContract user);

        [OperationContract]
        bool Remove(UserDataContract user);

        [OperationContract]
        IEnumerable<User> Search(UserDataContract user);

    }

    [DataContract]
    public class UserDataContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public List<VisaDataContract> VisaRecords { get; set; }

        [DataMember]
        public Gender Gender { get; set; }
    }

    [DataContract]
    public struct VisaDataContract
    {
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime End { get; set; }

    }
}
