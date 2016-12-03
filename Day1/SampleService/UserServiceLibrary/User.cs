using System;
using System.Collections.Generic;

namespace UserServiceLibrary
{
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public class User : IEquatable<User>
    {

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public Gender Gender { get; set; }
        public int Id { get; set; }

        public string LastName { get; set; }

        public List<VisaRecord> VisaRecords { get; set; }
        public static bool operator !=(User u1, User u2) => !u1.Equals(u2);

        public static bool operator ==(User u1, User u2) => u1.Equals(u2);

        public bool Equals(User other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (string.Compare(this.FirstName, other.FirstName, StringComparison.CurrentCulture) == 0 &&
                 string.Compare(this.LastName, other.LastName, StringComparison.CurrentCulture) == 0 &&
                 this.DateOfBirth.Equals(other.DateOfBirth))
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                if (FirstName != null) hash = hash * 23 + FirstName.GetHashCode();
                if (LastName != null) hash = hash * 23 + LastName.GetHashCode();
                hash = hash * 23 + Id.GetHashCode();
                if (DateOfBirth!=null) hash = hash * 23 + DateOfBirth.GetHashCode();
                hash = hash * 23 + Gender.GetHashCode();
                return hash;
            }
        }


    }
}
