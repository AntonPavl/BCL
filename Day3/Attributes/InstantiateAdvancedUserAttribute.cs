using System;
namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public int ExternalId { get; protected set; }
        public InstantiateAdvancedUserAttribute()
        {

        }
        public InstantiateAdvancedUserAttribute(int id,string fname,string lname,int externalId)
        {
            this.FirstName = fname;
            this.LastName = lname;
            this.ExternalId = externalId;
            this.Id = id;
        }

    }
}
