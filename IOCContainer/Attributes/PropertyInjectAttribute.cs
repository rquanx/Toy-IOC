using System;
using System.Collections.Generic;
using System.Text;

namespace IOCContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,AllowMultiple = false)]
    public sealed class PropertyInjectAttribute: Attribute
    {

    }
}
