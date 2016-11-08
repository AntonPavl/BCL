using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringValidatorAttribute : Attribute
    {
        public int maximumLength { get; set; }

    }
}
