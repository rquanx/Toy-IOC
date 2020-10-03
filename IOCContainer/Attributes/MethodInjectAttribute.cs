using System;
using System.Collections.Generic;
using System.Text;

namespace IOCContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false)]
    public sealed class MethodInjectAttribute: Attribute
    {
    }
}
