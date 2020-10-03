using System;
using System.Collections.Generic;
using System.Text;

namespace IOCContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor,AllowMultiple = false)]
    public sealed class ConstructorAttribute: Attribute
    {

    }
}
