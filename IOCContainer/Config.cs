using IOCContainer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOCContainer
{
    public class Config
    {
        public string FromDll { get; set; }
        public string From { get; set; }

        public string ToDll { get; set; }
        public string To { get; set; }
        public LifeTime LifeTime { get; set; }
        public object[] Args { get; set; }
    }
}
