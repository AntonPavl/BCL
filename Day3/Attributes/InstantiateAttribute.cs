using System;

namespace Attributes
{
    // Should be applied to classes only.
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class InstantiateUserAttribute : Attribute
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public int Id { get; protected set; }
        public InstantiateUserAttribute(string fname, string lname) : this (1, fname, lname)
        {

        }
        public InstantiateUserAttribute(int id,string fname, string lname)
        {
            this.FirstName = fname;
            this.LastName = lname;
            this.Id = id;
        }
        public InstantiateUserAttribute()
        {

        }
    }
}
