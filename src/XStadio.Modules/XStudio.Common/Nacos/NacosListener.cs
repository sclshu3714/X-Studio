using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common.Nacos
{
    public class NacosListener
    {
        public string DataId { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public bool Optional { get; set; } = false;
    }
}
