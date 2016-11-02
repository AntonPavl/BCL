using System;

namespace MyServiceLibrary
{
    public class User : IEquatable<User>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

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
    }
}
