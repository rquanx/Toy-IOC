using IOCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class ConstructorServiceA : IConstructorServiceA
    {

    }

    public class ConstructorServiceAA : IConstructorServiceA
    {

    }

    public class ConstructorServiceB : IConstructorServiceB
    {
        public IConstructorServiceA A { get; set; }
        public ConstructorServiceB(IConstructorServiceA a)
        {
            A = a;
        }
    }

    public class ConstructorServiceC : IConstructorServiceC
    {
        public IConstructorServiceA A { get; set; }
        public IConstructorServiceB B { get; set; }
        public ConstructorServiceC(IConstructorServiceA a, IConstructorServiceB b)
        {
            A = a;
            B = b;
        }
    }

    public class ConstructorServiceD : IConstructorServiceD
    {
        public IConstructorServiceA A { get; set; }
        public IConstructorServiceB B { get; set; }
        public IConstructorServiceC C { get; set; }
        public ConstructorServiceD(IConstructorServiceA a, IConstructorServiceB b)
        {
            A = a;
            B = b;
        }

        [Constructor]
        public ConstructorServiceD()
        {
        }
    }

    public class PropertyServiceA : IPropertyServiceA
    {
        public string Empty { get; set; }
    }
    public class PropertyServiceB : IPropertyServiceB
    {
        [PropertyInject]
        public IPropertyServiceA A { get; set; }
    }
    public class PropertyServiceC : IPropertyServiceC
    {
        [PropertyInject]
        public IPropertyServiceA A { get; set; }

        [PropertyInject]
        public IPropertyServiceB B { get; set; }
    }

    [PropertyInject]
    public class PropertyServiceD : IPropertyServiceD
    {

        public IPropertyServiceA A { get; set; }

        public IPropertyServiceB B { get; set; }
        public IPropertyServiceC C { get; set; }
    }


    public class MethodServiceA : IMethodServiceA
    {
        public string Empty { get; set; }

        public void Init()
        {
            Empty = "";
        }
    }
    public class MethodServiceB : IMethodServiceB
    {
        public IMethodServiceA A { get; set; }

        [MethodInject]
        public void Init(IMethodServiceA a)
        {
            A = a;
        }
    }
    public class MethodServiceC : IMethodServiceC
    {

        public IMethodServiceA A { get; set; }

        public IMethodServiceB B { get; set; }

        [MethodInject]
        public void InitA(IMethodServiceA a)
        {
            A = a;
        }

        [MethodInject]
        public void InitB(IMethodServiceB b)
        {
            B = b;
        }


    }

    public class MethodServiceD : IMethodServiceD
    {

        public IMethodServiceA A { get; set; }

        public IMethodServiceB B { get; set; }
        public IMethodServiceC C { get; set; }

        [MethodInject]
        public void InitA(IMethodServiceA a)
        {
            A = a;
        }
        [MethodInject]
        public void InitBC(IMethodServiceB b, IMethodServiceC c)
        {
            B = b;
            C = c;
        }
    }


    public class ConstructorParameterServiceA : IConstructorParameterServiceA
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }

        public ConstructorParameterServiceA([ParameterConstantInject] string str,
            [ParameterConstantInject] int num,
            [ParameterConstantInject] Dictionary<string, string> dic)
        {
            Str = str;
            Num = num;
            Dic = dic;
        }
    }

    public class ConstructorParameterServiceB : IConstructorParameterServiceB
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }
        public IConstructorParameterServiceA A { get; set; }
        public ConstructorParameterServiceB([ParameterConstantInject] string str, [ParameterConstantInject] int num,
            IConstructorParameterServiceA a, [ParameterConstantInject] Dictionary<string, string> dic)
        {
            A = a;
            Str = str;
            Num = num;
            Dic = dic;
        }
    }

    public class ConstructorParameterServiceC : IConstructorParameterServiceC
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }
        public IConstructorParameterServiceA A { get; set; }
        public IConstructorParameterServiceB B { get; set; }
        public ConstructorParameterServiceC(
            [ParameterConstantInject] string str,
            IConstructorParameterServiceA a, 
            [ParameterConstantInject] int num, 
            IConstructorParameterServiceB b, 
            [ParameterConstantInject] Dictionary<string, string> dic)
        {
            A = a;
            B = b;

            Str = str;
            Num = num;
            Dic = dic;
        }
    }

    public class ConstructorParameterServiceD : IConstructorParameterServiceD
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }
        public IConstructorParameterServiceA A { get; set; }
        public IConstructorParameterServiceB B { get; set; }
        public IConstructorParameterServiceC C { get; set; }
        public ConstructorParameterServiceD(IConstructorParameterServiceA a, IConstructorParameterServiceB b)
        {
            A = a;
            B = b;
        }

        [Constructor]
        public ConstructorParameterServiceD([ParameterConstantInject] string str,
            [ParameterConstantInject] int num,
            [ParameterConstantInject] Dictionary<string, string> dic)
        {
            Str = str;
            Num = num;
            Dic = dic;
        }
    }
}
