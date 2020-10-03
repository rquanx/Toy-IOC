using IOCContainer;
using IOCContainer.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void ConstructorInjectTest()
        {
            var container = new Container();
            container.Register<IConstructorServiceA, ConstructorServiceA>();
            container.Register<IConstructorServiceB, ConstructorServiceB>();
            container.Register<IConstructorServiceC, ConstructorServiceC>();
            container.Register<IConstructorServiceD, ConstructorServiceD>();

            // 无参构造函数
            var a = container.Resolve<IConstructorServiceA>();

            // 含依赖参数构造函数
            var b = container.Resolve<IConstructorServiceB>();

            // 含嵌套依赖参数构造函数
            var c = container.Resolve<IConstructorServiceC>();

            // 指定构造函数
            var d = container.Resolve<IConstructorServiceD>();


            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);

            Assert.NotNull(b.A);
            Assert.NotNull(c.A);
            Assert.NotNull(c.B);

            Assert.Null(d.C);
        }



        [Fact]
        public void PropertyInjectTest()
        {
            var container = new Container();
            container.Register<IPropertyServiceA, PropertyServiceA>();
            container.Register<IPropertyServiceB, PropertyServiceB>();
            container.Register<IPropertyServiceC, PropertyServiceC>();
            container.Register<IPropertyServiceD, PropertyServiceD>();
            // 无属性注入
            var a = container.Resolve<IPropertyServiceA>();

            // 含属性注入
            var b = container.Resolve<IPropertyServiceB>();

            // 含嵌套属性注入
            var c = container.Resolve<IPropertyServiceC>();

            // 类注入，全量属性注入
            var d = container.Resolve<IPropertyServiceD>();

            Assert.Null(a.Empty);

            Assert.NotNull(b.A);
            Assert.NotNull(c.A);
            Assert.NotNull(c.B);

            Assert.NotNull(d.A);
            Assert.NotNull(d.B);
            Assert.NotNull(d.C);

        }

        [Fact]
        public void MethodInjectTest()
        {
            var container = new Container();
            container.Register<IMethodServiceA, MethodServiceA>();
            container.Register<IMethodServiceB, MethodServiceB>();
            container.Register<IMethodServiceC, MethodServiceC>();
            container.Register<IMethodServiceD, MethodServiceD>();
            // 无方法注入
            var a = container.Resolve<IMethodServiceA>();

            // 含方法注入
            var b = container.Resolve<IMethodServiceB>();

            // 含嵌套方法注入
            var c = container.Resolve<IMethodServiceC>();

            var d = container.Resolve<IMethodServiceD>();

            Assert.Null(a.Empty);

            Assert.NotNull(b.A);
            Assert.NotNull(c.A);
            Assert.NotNull(c.B);

            Assert.NotNull(d.A);
            Assert.NotNull(d.B);
            Assert.NotNull(d.C);

        }

        [Fact]
        public void ParameterInjectTest()
        {
            var container = new Container();
            container.Register<IConstructorParameterServiceA, ConstructorParameterServiceA>(
                new object[] { "123", 1, new Dictionary<string, string>() }
                );
            container.Register<IConstructorParameterServiceB, ConstructorParameterServiceB>(
                new object[] { "123", 1, new Dictionary<string, string>() }
                );
            container.Register<IConstructorParameterServiceC, ConstructorParameterServiceC>(
                new object[] { "123", 1, new Dictionary<string, string>() }
                );
            container.Register<IConstructorParameterServiceD, ConstructorParameterServiceD>(
                new object[] { "123", 1, new Dictionary<string, string>() }
                );

            // 无参构造函数
            var a = container.Resolve<IConstructorParameterServiceA>();

            // 含依赖参数构造函数
            var b = container.Resolve<IConstructorParameterServiceB>();

            // 含嵌套依赖参数构造函数
            var c = container.Resolve<IConstructorParameterServiceC>();

            // 指定构造函数
            var d = container.Resolve<IConstructorParameterServiceD>();

            Assert.NotNull(a.Str);
            Assert.NotNull(b.Str);
            Assert.NotNull(c.Str);
            Assert.NotNull(d.Str);

            Assert.True(a.Num > 0);
            Assert.True(b.Num > 0);
            Assert.True(c.Num > 0);
            Assert.True(d.Num > 0);

            Assert.NotNull(a.Dic);
            Assert.NotNull(b.Dic);
            Assert.NotNull(c.Dic);
            Assert.NotNull(d.Dic);

        }

        [Fact]
        public void MultipleInjectTest()
        {
            var container = new Container();
            container.Register<IConstructorServiceA, ConstructorServiceA>();
            container.Register<IConstructorServiceA, ConstructorServiceAA>();
            var a = container.Resolve<IConstructorServiceA>();
            var a1 = container.Resolve<IConstructorServiceA>(typeof(ConstructorServiceA));
            var a2 = container.Resolve<IConstructorServiceA>(typeof(ConstructorServiceAA));
            var a3 = container.Resolve<IConstructorServiceA>(typeof(ConstructorServiceA).FullName);
            var a4 = container.Resolve<IConstructorServiceA>(typeof(ConstructorServiceAA).FullName);

            Assert.IsType<ConstructorServiceA>(a);
            Assert.IsType<ConstructorServiceA>(a1);
            Assert.IsType<ConstructorServiceAA>(a2);
            Assert.IsType<ConstructorServiceA>(a3);
            Assert.IsType<ConstructorServiceAA>(a4);
        }
    
        [Fact]
        public void LifeTimeTest()
        {
            var container = new Container();
            container.Register<IConstructorServiceA, ConstructorServiceA>(lifeTime: LifeTime.Singlenton);
            container.Register<IConstructorServiceB, ConstructorServiceB>();
            container.Register<IConstructorServiceC, ConstructorServiceC>(lifeTime: LifeTime.Scope);

            var a1 = container.Resolve<IConstructorServiceA>();
            var a2 = container.Resolve<IConstructorServiceA>();

            Assert.Equal(a1, a2);

            var b1 = container.Resolve<IConstructorServiceB>();
            var b2 = container.Resolve<IConstructorServiceB>();

            Assert.NotEqual(b1, b2);

            var c1 = container.Resolve<IConstructorServiceC>();
            var c2 = container.Resolve<IConstructorServiceC>();
            Assert.Equal(c1, c2);

            var container1 = container.Clone();

            var a11 = container.Resolve<IConstructorServiceA>();
            var a12 = container.Resolve<IConstructorServiceA>();
            Assert.Equal(a11, a12);

            var c11 = container1.Resolve<IConstructorServiceC>();
            var c12 = container1.Resolve<IConstructorServiceC>();
            Assert.Equal(c11, c12);
            Assert.NotEqual(c11, c1);

        }
    }
}
