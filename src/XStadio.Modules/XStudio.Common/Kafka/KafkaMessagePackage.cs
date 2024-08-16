using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus;

namespace XStudio.Common.Kafka
{
    [EventName("XStudio.Kafka.Message")]
    public class KafkaMessagePackage
    {
        public KafkaMessagePackage(string message) { 
            Message = message;
        }

        public string Message { get; set; }
    }
}
