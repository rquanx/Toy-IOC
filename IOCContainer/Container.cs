using IOCContainer.Attributes;
using IOCContainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IOCContainer
{
    public class Container
    {
        /// <summary>
        /// 注册容器
        /// </summary>
        private readonly Dictionary<string, RegisterModel> Contain = new Dictionary<string, RegisterModel>();

        /// <summary>
        /// 构造函数参数容器
        /// </summary>
        private readonly Dictionary<string, object[]> ParametersContain = new Dictionary<string, object[]>();

        /// <summary>
        /// Scope实例实例，每次Clone都产生新的Scope容器，同一个容器时总是返回同一个Scope的实例
        /// </summary>
        private readonly Dictionary<string, object> ScopeContain = new Dictionary<string, object>();

        /// <summary>
        /// 复制映射，用于Scope
        /// </summary>
        /// <returns></returns>
        public Container Clone()
        {
            return new Container(Contain, ParametersContain, new Dictionary<string, object>());
        }

        public Container() { }

        /// <summary>
        /// 根据配置文件注册映射
        /// </summary>
        /// <param name="configs"></param>
        public Container(Config[] configs)
        {
            foreach (var config in configs)
            {
                var fromAssembly = Assembly.LoadFile(config.FromDll);
                var from = fromAssembly.GetTypes().Where((item) => item.FullName == config.From).FirstOrDefault();
                var toAssembly = Assembly.LoadFile(config.FromDll);
                var to = toAssembly.GetTypes().Where((item) => item.FullName == config.ToDll).FirstOrDefault();

                // 通过Type调用泛型方法
                GetType().GetMethod("Register")
                    .MakeGenericMethod(from, to).Invoke(this,
                new object[] { config.Args, config.LifeTime });
            }
        }

        private Container(Dictionary<string, RegisterModel> contain,
            Dictionary<string, object[]> parametersContain,
            Dictionary<string, object> scopeContain)
        {
            this.Contain = contain;
            this.ParametersContain = parametersContain;
            this.ScopeContain = scopeContain;
        }

        private string GetKey(Type typeFrom, Type typeTo)
        {
            return GetKey(typeFrom, typeTo?.FullName);
        }

        private string GetKey(Type typeFrom, string toName)
        {
            if (toName != null) { return $@"{typeFrom.FullName}___{toName}"; }
            return Contain.Keys.Where((key) => key.StartsWith($"{typeFrom.FullName}___")).FirstOrDefault();
        }

        public void Register<IFrom, To>(object[] args = null, LifeTime lifeTime = LifeTime.Transient) where To : IFrom
        {
            var typeFrom = typeof(IFrom);
            var typeTo = typeof(To);
            var name = GetKey(typeFrom, typeTo);
            if (Contain.ContainsKey(name))
            {
                Contain[name] = new RegisterModel()
                {
                    Target = typeTo,
                    LifeTime = lifeTime,
                };
                ParametersContain[name] = args;
            }
            else
            {
                Contain.Add(name, new RegisterModel()
                {
                    Target = typeTo,
                    LifeTime = lifeTime,
                });
                ParametersContain.Add(name, args);
            }
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            var constructors = type.GetConstructors();
            var constructor = constructors.Where((item) => item.IsDefined(typeof(ConstructorAttribute), false)).FirstOrDefault();
            if (constructor != null) return constructor;
            return constructors.OrderByDescending((item) => item.GetParameters().Length).FirstOrDefault();
        }

        private void PropertyInject(object instance)
        {
            var type = instance.GetType();
            var classFlag = type.IsDefined(typeof(PropertyInjectAttribute), false);
            foreach (var prop in type.GetProperties())
            {
                if (classFlag || prop.IsDefined(typeof(PropertyInjectAttribute), false))
                {
                    prop.SetValue(instance, ResolveObject(prop.PropertyType));
                }
            }
        }

        private void MethodInject(object instance)
        {
            var type = instance.GetType();
            var classFlag = type.IsDefined(typeof(MethodInjectAttribute), false);
            // TODO: 看看是否有较好的方式过滤掉一些难以处理的方法
            // 好像没有
            // 区分继承的方法
            // 区分自身普通方法和get、set
            //foreach (var method in type.GetMethods())
            //{
            //if (classFlag || method.IsDefined(typeof(MethodInjectAttribute), false))
            //{
            //    var args = new List<object>();
            //    foreach (var param in method.GetParameters())
            //    {
            //        args.Add(ResolveObject(param.ParameterType));
            //    }
            //    method.Invoke(instance, args.ToArray());
            //}
            //}

            foreach (var method in type.GetMethods().Where((m) => m.IsDefined(typeof(MethodInjectAttribute), false)))
            {
                var args = new List<object>();
                foreach (var param in method.GetParameters())
                {
                    args.Add(ResolveObject(param.ParameterType));
                }
                method.Invoke(instance, args.ToArray());
            }
        }

        private object CreateInstance(Type type, object[] parameters = null)
        {
            var constructor = GetConstructor(type);
            List<object> args = new List<object>();
            var index = 0;
            foreach (var param in constructor.GetParameters())
            {
                if (parameters != null && param.IsDefined(typeof(ParameterConstantInjectAttribute), false))
                {
                    args.Add(parameters[index++]);
                }
                else
                {
                    args.Add(ResolveObject(param.ParameterType));
                }
            }
            var instance = Activator.CreateInstance(type, args.ToArray());
            PropertyInject(instance);
            MethodInject(instance);
            return instance;
        }

        public IFrom Resolve<IFrom>(Type typeTo = null) where IFrom : class
        {
            var typeFrom = typeof(IFrom);
            return ResolveObject(typeFrom, typeTo) as IFrom;
        }

        public IFrom Resolve<IFrom>(string toName) where IFrom : class
        {
            var typeFrom = typeof(IFrom);
            return ResolveObject(typeFrom, toName) as IFrom;
        }


        private object ResolveObject(Type typeFrom, Type typeTo = null)
        {
            var name = GetKey(typeFrom, typeTo);
            return ResolveObject(name);
        }

        private object ResolveObject(Type typeFrom, string toName)
        {
            var name = GetKey(typeFrom, toName);
            return ResolveObject(name);
        }


        private object ResolveObject(string fullName)
        {
            var name = fullName;
            if (!Contain.ContainsKey(name)) return null;
            var model = Contain[name];
            object result;
            switch (model.LifeTime)
            {
                case LifeTime.Singlenton:
                    result = ResolveSinglenton(model, name);
                    break;
                case LifeTime.Scope:
                    result = ResolveScope(model, name);
                    break;
                case LifeTime.Transient:
                default:
                    result = CreateInstance(model.Target, ParametersContain[name]);
                    break;
            }
            return result;
        }

        private object ResolveScope(RegisterModel model, string name)
        {
            if (ScopeContain.ContainsKey(name))
            {
                return ScopeContain[name];
            }
            var instance = CreateInstance(model.Target, ParametersContain[name]);
            ScopeContain.Add(name, instance);
            return instance;
        }

        private object ResolveSinglenton(RegisterModel model, string name)
        {
            if (model.Instance != null) return model.Instance;
            var instance = CreateInstance(model.Target, ParametersContain[name]);
            model.Instance = instance;
            return instance;
        }
    }
}
