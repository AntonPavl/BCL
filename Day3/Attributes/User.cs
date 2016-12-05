using System;
using System.ComponentModel;
using System.Reflection;
namespace Attributes
{
   // [InstantiateUser("Alexander", "Alexandrov")]
   // [InstantiateUser(2, "Semen", "Semenov")]
    [InstantiateUser(3, "Petr", "Petrov")]
    public class User
    {
        [IntValidator(1, 1000)]
        private int _id;

        [DefaultValue(1)]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        [StringValidator(maximumLength = 30)]
        public string FirstName { get; set; }

        [StringValidator(maximumLength = 20)]
        public string LastName { get; set; }

      //  [MatchParameterWithProperty("id", "Id")]
        public User(int id)
        {
            _id = id;
        }

        public bool IsValid()
        {
            //var t = this.GetType().GetField(nameof(_id));
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.Name == nameof(FirstName))
                {
                    foreach (Attribute att in prop.GetCustomAttributes(false))
                    {
                        var c = att as StringValidatorAttribute;
                        if (c != null)
                        { 
                            if (this.FirstName.Length > c.maximumLength)
                            {
                                // throw new ArgumentException($"Length must be > {c.maximumLength}");
                                return false;
                            }
                        }
                    }
                }
                else
                if (prop.Name == nameof(LastName))
                {
                    foreach (Attribute att in prop.GetCustomAttributes(false))
                    {
                        var c = att as StringValidatorAttribute;
                        if (c != null)
                        {
                            if (this.LastName.Length > c.maximumLength)
                            {
                                // throw new ArgumentException($"Length must be > {c.maximumLength}");
                                return false;
                            }
                        }
                    }
                }
            }
            var idType = GetType().GetField(nameof(_id), BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (Attribute att in idType.GetCustomAttributes(false))
            {
                var c = att as IntValidatorAttribute;
                if (c != null)
                {
                    if (this._id > c.IdMax || this._id < c.IdMin)
                    {
                        // throw new ArgumentException($"Length must be > {c.maximumLength}");
                        return false;
                    }
                }
            }
            return true;
        }

    }
}