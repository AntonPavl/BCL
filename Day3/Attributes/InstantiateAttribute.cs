using System;

namespace Attributes
{
    // Should be applied to classes only.
    [AttributeUsage(AttributeTargets.Class)]
    public class InstantiateUserAttribute : Attribute
    {
    }
}
