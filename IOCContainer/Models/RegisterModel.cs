using System;
using System.Collections.Generic;
using System.Text;

namespace IOCContainer.Models
{
    public class RegisterModel
    {
        public Type Target { get; set; }

        public LifeTime LifeTime { get; set; }

        public object Instance { get; set; }

    }

    public enum LifeTime
    {
        /// <summary>
        /// 每次都是新的
        /// </summary>
        Transient,
        /// <summary>
        /// 单例
        /// </summary>
        Singlenton,
        /// <summary>
        /// 
        /// </summary>
        Scope
    }
}
