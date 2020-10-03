using System;
using System.Collections.Generic;
using System.Text;

namespace IOCContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = false)]
    public sealed class ParameterConstantInjectAttribute: Attribute
    {

    }
}
