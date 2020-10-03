using IOCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public interface IConstructorServiceA
    {

    }

    public interface IConstructorServiceB
    {
        public IConstructorServiceA A { get; set; }
    }

    public interface IConstructorServiceC
    {
        public IConstructorServiceA A { get; set; }
        public IConstructorServiceB B { get; set; }
    }

    public interface IConstructorServiceD
    {
        public IConstructorServiceA A { get; set; }
        public IConstructorServiceB B { get; set; }
        public IConstructorServiceC C { get; set; }
    }


    public interface IPropertyServiceA
    {
        public string Empty { get; set; }
    }

    public interface IPropertyServiceB
    {
       
        public IPropertyServiceA A { get; set; }
    }

    public interface IPropertyServiceC
    {
       
        public IPropertyServiceA A { get; set; }
        
        
        public IPropertyServiceB B { get; set; }
    }

    public interface IPropertyServiceD
    {

        public IPropertyServiceA A { get; set; }

        public IPropertyServiceB B { get; set; }

        public IPropertyServiceC C { get; set; }
    }

    public interface IMethodServiceA
    {
        public string Empty { get; set; }

        public void Init();
    }

    public interface IMethodServiceB
    {
       
        public IMethodServiceA A { get; set; }

        public void Init(IMethodServiceA a);
    }

    public interface IMethodServiceC
    {
       
        public IMethodServiceA A { get; set; }
        
        
        public IMethodServiceB B { get; set; }

        public void InitA(IMethodServiceA a);

        public void InitB(IMethodServiceB b);
    }

    public interface IMethodServiceD
    {

        public IMethodServiceA A { get; set; }

        public IMethodServiceB B { get; set; }

        public IMethodServiceC C { get; set; }

        public void InitA(IMethodServiceA a);

        public void InitBC(IMethodServiceB b,IMethodServiceC c);
    }

    public interface IConstructorParameterServiceA
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string,string> Dic { get; set; }
        
    }

    public interface IConstructorParameterServiceB
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }
        public IConstructorParameterServiceA A { get; set; }
    }

    public interface IConstructorParameterServiceC
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }
        public IConstructorParameterServiceA A { get; set; }
        public IConstructorParameterServiceB B { get; set; }
    }

    public interface IConstructorParameterServiceD
    {
        public string Str { get; set; }
        public int Num { get; set; }
        public Dictionary<string, string> Dic { get; set; }
        public IConstructorParameterServiceA A { get; set; }
        public IConstructorParameterServiceB B { get; set; }
        public IConstructorParameterServiceC C { get; set; }
    }
}
