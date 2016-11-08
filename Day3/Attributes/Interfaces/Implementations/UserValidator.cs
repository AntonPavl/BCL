using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Attributes.Interfaces.Implementations
{
    public class UserValidator : IUserValidator
    {
        public bool IsValid(User user)
        {
            PropertyInfo prop;
            FieldInfo field;
            StringValidatorAttribute attr;
            IntValidatorAttribute iattr;
            if (user.FirstName != null)
            {
                prop = user.GetType().GetProperty("FirstName");
                attr = (StringValidatorAttribute)Attribute.GetCustomAttribute(prop, typeof(StringValidatorAttribute));
                if (attr.maximumLength < user.FirstName.Length) return false;
            }
            if (user.LastName != null)
            {
                prop = user.GetType().GetProperty("LastName");
                attr = (StringValidatorAttribute)Attribute.GetCustomAttribute(prop, typeof(StringValidatorAttribute));
                if (attr.maximumLength < user.FirstName.Length) return false;
            }
            field = user.GetType().GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance);
            iattr = (IntValidatorAttribute)Attribute.GetCustomAttribute(field, typeof(IntValidatorAttribute));
            if (user.Id > iattr.IdMax || user.Id < iattr.IdMin) return false;

            return true;
        }
    }
}
