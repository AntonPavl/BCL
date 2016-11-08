using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IntValidatorAttribute : Attribute
    {
        public int IdMax { get; set; }
        public int IdMin { get; set; }
        public IntValidatorAttribute(int min, int max)
        {
            IdMax = max;
            IdMin = min;
        }
    }
}